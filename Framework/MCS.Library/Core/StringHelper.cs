using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MCS.Library.Core
{
    /// <summary>
    /// 身份证验证结果
    /// </summary>
    [Flags]
    public enum CheckCidResult
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,
        /// <summary>
        /// 身份证验证成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 身份证串非法
        /// </summary>
        ErrorString = 2,
        /// <summary>
        /// 身份证地区非法
        /// </summary>
        ErrorProvince = 4,
        /// <summary>
        /// 身份证生日非法
        /// </summary>
        ErrorBirthay = 8,
        /// <summary>
        /// 身份证验证码非法
        /// </summary>
        ErrorCard = 16
    }

    /// <summary>
    /// 提供用于设置转换选项的枚举值。
    /// </summary>
    [Flags]
    public enum HZSpellOption
    {
        /// <summary>
        /// 只转换拼音首字母，默认转换全部
        /// </summary>
        FirstLetterOnly = 1,
        /// <summary>
        /// 拼音中首字母大些
        /// </summary>
        FirstLetterUpper = 1 << 1,
        /// <summary>
        /// 转换未知汉字为问号，默认不转换
        /// </summary>
        EnableUnknownWord = 1 << 2,
        /// <summary>
        /// 保留非字母、非数字字符，默认不保留
        /// </summary>
        EnableUnicodeLetter = 1 << 4
    }

    /// <summary>
    /// 此Class提供常用字符串数据的综合处理
    /// </summary>
    /// <remarks>
    /// 处理内容包括有：
    /// 1、半角全角数据的转换；
    /// 2、中国大陆身份证的验证以及15位转换18位的实现；
    /// 3、汉字拼音转换实现；
    /// </remarks>
    public static class StringHelper
    {
        #region 全角半角转换
        /// <summary>
        /// 全角转半角的函数(DBC case)
        /// </summary>
        /// <param name="source">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string source)
        {
            char[] temp = source.ToCharArray();

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == 12288)
                {
                    temp[i] = (char)32;
                    continue;
                }
                if (temp[i] > 65280 && temp[i] < 65375)
                    temp[i] = (char)(temp[i] - 65248);
            }
            return new string(temp);

        }

        /// <summary>
        /// 半角转全角的函数(SBC case)
        /// </summary>
        /// <param name="source">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks> 
        public static string ToSBC(string source)
        {
            char[] temp = source.ToCharArray();

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == 32)
                {
                    temp[i] = (char)12288;
                    continue;
                }
                if (temp[i] < 127)
                    temp[i] = (char)(temp[i] + 65248);
            }
            return new string(temp);
        }
        #endregion

        #region 身份证处理
        #region Province Define
        private readonly static string[] chinaCities = new string[]{
				null,	null,	null,	null,	null,	null,	null,	null,	null,	null,
				null,	"北京",	"天津",	"河北",	"山西",	"内蒙古",null,	null,	null,	null,
				null,	"辽宁",	"吉林",	"黑龙江",null,	null,	null,	null,	null,	null,
				null,	"上海",	"江苏",	"浙江",	"安微",	"福建",	"江西",	"山东",	null,	null,
				null,	"河南",	"湖北",	"湖南",	"广东",	"广西",	"海南",	null,	null,	null,
				"重庆",	"四川",	"贵州",	"云南",	"西藏",	null,	null,	null,	null,	null,
				null,	"陕西",	"甘肃",	"青海",	"宁夏",	"新疆",	null,	null,	null,	null,
				null,	"台湾",	null,	null,	null,	null,	null,	null,	null,	null,
				null,	"香港",	"澳门",	null,	null,	null,	null,	null,	null,	null,
				null,	"国外"};
        #endregion

        /// <summary>
        /// 中国大陆身份证验证程序
        /// </summary>
        /// <param name="userCid">待验证的用户身份证编号</param>
        /// <returns>检查结果</returns>
        /// <remarks>
        /// 根据〖中华人民共和国国家标准 GB 11643-1999〗中有关公民身份号码的规定，公民身份号码是特征组合码，由十七位数字本体码和一位数字校验码组成。
        /// 排列顺序从左至右依次为：六位数字地址码，八位数字出生日期码，三位数字顺序码和一位数字校验码。 
        /// 地址码 : 表示编码对象常住户口所在县(市、旗、区)的行政区划代码,按gb/t2260的规定执行。
        /// 生日期码 : 表示编码对象出生的年、月、日，其中年份用四位数字表示，年、月、日之间不用分隔符。
        /// 顺序码 : 表示同一地址码所标识的区域范围内，对同年、月、日出生的人员编定的顺序号。顺序码的奇数分给男性，偶数分给女性。
        /// 校验码 : 是根据前面十七位数字码，按照ISO 7064:1983.MOD 11-2校验码计算出来的检验码。
        /// 
        /// 15位的身份证编码首先把出生年扩展为4位，简单的就是增加一个19，但是这对于1900年出生的人不使用（这样的寿星不多了） 
        /// 
        /// 某男性公民身份号码本体码为34052419800101001?，首先按照公式⑴计算： 
        /// ∑(ai×Wi)(mod 11)……………………………………(1) 
        /// 
        /// 公式(1)中： 
        /// i----表示号码字符从由至左包括校验码在内的位置序号； 
        /// ai----表示第i位置上的号码字符值； 
        /// Wi----示第i位置上的加权因子，其数值依据公式Wi=2（n-1）(mod 11)计算得出。
        /// i		18	17	16	15	14	13	12	11	10	9	8	7	6	5	4	3	2	1 
        /// ai		3	4	0	5	2	4	1	9	8	0	0	1	0	1	0	0	1	? 
        /// Wi		7	9	10	5	8	4	2	1	6	3	7	9	10	5	8	4	2	 
        /// ai×Wi	21	36	0	25	16	16	2	9	48	0	0	9	0	5	0	0	2	? 
        /// 
        /// 根据公式(1)进行计算：
        /// ∑(ai×Wi) =（21+36+0+25+16+16+2+9+48++0+0+9+0+5+0+0+2) = 189 
        /// 189 ÷ 11 = 17 + 2/11 
        /// ∑(ai×Wi)(mod 11) = 2 
        /// 然后根据计算的结果，从下面的表中查出相应的校验码，其中X表示计算结果为10： 
        /// ∑(ai×WI)(mod 11) 0 1 2 3 4 5 6 7 8 9 10 
        /// 校验码字符值ai 1 0 X 9 8 7 6 5 4 3 2 
        /// 根据上表，查出计算结果为2的校验码为所以该人员的公民身份号码应该为 34052419800101001X
        /// </remarks>
        public static CheckCidResult CheckCidInfo(string userCid)
        {
            Regex rg;
            if (userCid.Length == 15)
                rg = new Regex(@"^\d{15}$");
            else
                rg = new Regex(@"^\d{17}(\d|x)$");
            Match mc = rg.Match(userCid);
            if (false == mc.Success)
                return CheckCidResult.ErrorString;

            //string city = chinaCities[int.Parse(userCid.Substring(0, 2))];
            int sub = (userCid.Length == 15 ? 0 : 2);
            string birthday = (userCid.Length == 15 ? "19" : string.Empty) +
                userCid.Substring(6, 2 + sub) + "-" + userCid.Substring(8 + sub, 2) + "-" + userCid.Substring(10 + sub, 2);

            userCid = userCid.ToLower().Replace("x", "a");

            if (chinaCities[int.Parse(userCid.Substring(0, 2))] == null)
                return CheckCidResult.ErrorProvince;

            try
            {
                DateTime.Parse(birthday);
            }
            catch
            {
                return CheckCidResult.ErrorBirthay;
            }

            if (userCid.Length == 18)
            {
                double iSum = 0;
                for (int i = 17; i >= 0; i--)
                {
                    iSum += (System.Math.Pow(2, i) % 11) * int.Parse(userCid[17 - i].ToString(), NumberStyles.HexNumber);
                }

                if (iSum % 11 != 1)
                    return CheckCidResult.ErrorCard;
            }
            //string sex = int.Parse(userCid.Substring(14 + sub, 1)) % 2 == 1 ? "男" : "女";
            //message = city + "," + birthday + "," + sex;

            return CheckCidResult.Success;
        }

        /// <summary>
        /// 中国大陆身份证15位转换位18位
        /// </summary>
        /// <param name="userCid15">待转换的15位身份证</param>
        /// <returns>转换成功的身份证编号</returns>
        /// <remarks>
        /// 根据〖中华人民共和国国家标准 GB 11643-1999〗中有关公民身份号码的规定，公民身份号码是特征组合码，由十七位数字本体码和一位数字校验码组成。
        /// 排列顺序从左至右依次为：六位数字地址码，八位数字出生日期码，三位数字顺序码和一位数字校验码。 
        /// 地址码 : 表示编码对象常住户口所在县(市、旗、区)的行政区划代码,按gb/t2260的规定执行。
        /// 生日期码 : 表示编码对象出生的年、月、日，其中年份用四位数字表示，年、月、日之间不用分隔符。
        /// 顺序码 : 表示同一地址码所标识的区域范围内，对同年、月、日出生的人员编定的顺序号。顺序码的奇数分给男性，偶数分给女性。
        /// 校验码 : 是根据前面十七位数字码，按照ISO 7064:1983.MOD 11-2校验码计算出来的检验码。
        /// 
        /// 15位的身份证编码首先把出生年扩展为4位，简单的就是增加一个19，但是这对于1900年出生的人不使用（这样的寿星不多了） 
        /// 
        /// 某男性公民身份号码本体码为34052419800101001?，首先按照公式⑴计算： 
        /// ∑(ai×Wi)(mod 11)……………………………………(1) 
        /// 
        /// 公式(1)中： 
        /// i----表示号码字符从由至左包括校验码在内的位置序号； 
        /// ai----表示第i位置上的号码字符值； 
        /// Wi----示第i位置上的加权因子，其数值依据公式Wi=2（n-1）(mod 11)计算得出。
        /// i		18	17	16	15	14	13	12	11	10	9	8	7	6	5	4	3	2	1 
        /// ai		3	4	0	5	2	4	1	9	8	0	0	1	0	1	0	0	1	? 
        /// Wi		7	9	10	5	8	4	2	1	6	3	7	9	10	5	8	4	2	 
        /// ai×Wi	21	36	0	25	16	16	2	9	48	0	0	9	0	5	0	0	2	? 
        /// 
        /// 根据公式(1)进行计算：
        /// ∑(ai×Wi) =（21+36+0+25+16+16+2+9+48++0+0+9+0+5+0+0+2) = 189 
        /// 189 ÷ 11 = 17 + 2/11 
        /// ∑(ai×Wi)(mod 11) = 2 
        /// 然后根据计算的结果，从下面的表中查出相应的校验码，其中X表示计算结果为10： 
        /// ∑(ai×WI)(mod 11) 0 1 2 3 4 5 6 7 8 9 10 
        /// 校验码字符值ai 1 0 X 9 8 7 6 5 4 3 2 
        /// 根据上表，查出计算结果为2的校验码为所以该人员的公民身份号码应该为 34052419800101001X
        /// </remarks>
        public static string ConvertCid15to18(string userCid15)
        {
            CheckCidResult check = CheckCidInfo(userCid15);
            ExceptionHelper.TrueThrow<ArgumentException>(check == CheckCidResult.ErrorBirthay, "原始身份证{0}编码生日非法", "userCid15");
            ExceptionHelper.TrueThrow<ArgumentException>(check == CheckCidResult.ErrorProvince, "原始身份证{0}编码地区非法", "userCid15");
            ExceptionHelper.TrueThrow<ArgumentException>(check == CheckCidResult.ErrorString, "原始身份证{0}编码非法", "userCid15");
            //加权因子常数 
            int[] wi = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数 
            string lastCode = "10x98765432";
            //新身份证号 : 填在第6位及第7位上填上‘1’，‘9’两个数字 
            string peridnew = userCid15.Substring(0, 6) + "19" + userCid15.Substring(6, 9);

            int x = 0;
            //进行加权求和 
            for (int i = 0; i < 17; i++)
            {
                int ai = int.Parse(peridnew.Substring(i, 1));
                x += ai * wi[i];
            }

            //取模运算，得到模值 
            x = x % 11;
            //从lastcode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。 
            return peridnew + lastCode.Substring(x, 1);
        }
        #endregion

        #region 汉字拼音转换
        #region 内码定义
        private readonly static int[] pyvalue = new int[]{
				-20319,	-20317,	-20304,	-20295,	-20292,	-20283,	-20265,	-20257,	-20242,	-20230,	
				-20051,	-20036,	-20032,	-20026,	-20002,	-19990,	-19986,	-19982,	-19976,	-19805,
				-19784,	-19775,	-19774,	-19763,	-19756,	-19751,	-19746,	-19741,	-19739,	-19728,
				-19725,	-19715,	-19540,	-19531,	-19525,	-19515,	-19500,	-19484,	-19479,	-19467,
				-19289,	-19288,	-19281,	-19275,	-19270,	-19263,	-19261,	-19249,	-19243,	-19242,
				-19238,	-19235,	-19227,	-19224,	-19218,	-19212,	-19038,	-19023,	-19018,	-19006,
				-19003,	-18996,	-18977,	-18961,	-18952,	-18783,	-18774,	-18773,	-18763,	-18756,
				-18741,	-18735,	-18731,	-18722,	-18710,	-18697,	-18696,	-18526,	-18518,	-18501,
				-18490,	-18478,	-18463,	-18448,	-18447,	-18446,	-18239,	-18237,	-18231,	-18220,
				-18211,	-18201,	-18184,	-18183,	-18181,	-18012,	-17997,	-17988,	-17970,	-17964,
				-17961,	-17950,	-17947,	-17931,	-17928,	-17922,	-17759,	-17752,	-17733,	-17730,
				-17721,	-17703,	-17701,	-17697,	-17692,	-17683,	-17676,	-17496,	-17487,	-17482,	
				-17468,	-17454,	-17433,	-17427,	-17417,	-17202,	-17185,	-16983,	-16970,	-16942,	
				-16915,	-16733,	-16708,	-16706,	-16689,	-16664,	-16657,	-16647,	-16474,	-16470,	
				-16465,	-16459,	-16452,	-16448,	-16433,	-16429,	-16427,	-16423,	-16419,	-16412,	
				-16407,	-16403,	-16401,	-16393,	-16220,	-16216,	-16212,	-16205,	-16202,	-16187,	
				-16180,	-16171,	-16169,	-16158,	-16155,	-15959,	-15958,	-15944,	-15933,	-15920,	
				-15915,	-15903,	-15889,	-15878,	-15707,	-15701,	-15681,	-15667,	-15661,	-15659,	
				-15652,	-15640,	-15631,	-15625,	-15454,	-15448,	-15436,	-15435,	-15419,	-15416,	
				-15408,	-15394,	-15385,	-15377,	-15375,	-15369,	-15363,	-15362,	-15183,	-15180,	
				-15165,	-15158,	-15153,	-15150,	-15149,	-15144,	-15143,	-15141,	-15140,	-15139,	
				-15128,	-15121,	-15119,	-15117,	-15110,	-15109,	-14941,	-14937,	-14933,	-14930,	
				-14929,	-14928,	-14926,	-14922,	-14921,	-14914,	-14908,	-14902,	-14894,	-14889,	
				-14882,	-14873,	-14871,	-14857,	-14678,	-14674,	-14670,	-14668,	-14663,	-14654,	
				-14645,	-14630,	-14594,	-14429,	-14407,	-14399,	-14384,	-14379,	-14368,	-14355,	
				-14353,	-14345,	-14170,	-14159,	-14151,	-14149,	-14145,	-14140,	-14137,	-14135,	
				-14125,	-14123,	-14122,	-14112,	-14109,	-14099,	-14097,	-14094,	-14092,	-14090,	
				-14087,	-14083,	-13917,	-13914,	-13910,	-13907,	-13906,	-13905,	-13896,	-13894,	
				-13878,	-13870,	-13859,	-13847,	-13831,	-13658,	-13611,	-13601,	-13406,	-13404,	
				-13400,	-13398,	-13395,	-13391,	-13387,	-13383,	-13367,	-13359,	-13356,	-13343,	
				-13340,	-13329,	-13326,	-13318,	-13147,	-13138,	-13120,	-13107,	-13096,	-13095,	
				-13091,	-13076,	-13068,	-13063,	-13060,	-12888,	-12875,	-12871,	-12860,	-12858,	
				-12852,	-12849,	-12838,	-12831,	-12829,	-12812,	-12802,	-12607,	-12597,	-12594,	
				-12585,	-12556,	-12359,	-12346,	-12320,	-12300,	-12120,	-12099,	-12089,	-12074,	
				-12067,	-12058,	-12039,	-11867,	-11861,	-11847,	-11831,	-11798,	-11781,	-11604,	
				-11589,	-11536,	-11358,	-11340,	-11339,	-11324,	-11303,	-11097,	-11077,	-11067,	
				-11055,	-11052,	-11045,	-11041,	-11038,	-11024,	-11020,	-11019,	-11018,	-11014,	
				-10838,	-10832,	-10815,	-10800,	-10790,	-10780,	-10764,	-10587,	-10544,	-10533,	
				-10519,	-10331,	-10329,	-10328,	-10322,	-10315,	-10309,	-10307,	-10296,	-10281,	
				-10274,	-10270,	-10262,	-10260,	-10256,	-10254};
        #endregion

        #region 拼音码定义
        /// <summary>
        /// 拼音代码表
        /// </summary>
        private static readonly string[] pystr = new string[]{
				"a",	"ai",	"an",	"ang",	"ao",	"ba",	"bai",	"ban",	"bang",	"bao",	
				"bei",	"ben",	"beng",	"bi",	"bian",	"biao",	"bie",	"bin",	"bing",	"bo",	
				"bu",	"ca",	"cai",	"can",	"cang",	"cao",	"ce",	"ceng",	"cha",	"chai",	
				"chan",	"chang","chao",	"che",	"chen",	"cheng","chi",	"chong","chou",	"chu",	
				"chuai","chuan","chuang","chui","chun",	"chuo",	"ci",	"cong",	"cou",	"cu",	
				"cuan",	"cui",	"cun",	"cuo",	"da",	"dai",	"dan",	"dang",	"dao",	"de",	
				"deng",	"di",	"dian",	"diao",	"die",	"ding",	"diu",	"dong",	"dou",	"du",	
				"duan",	"dui",	"dun",	"duo",	"e",	"en",	"er",	"fa",	"fan",	"fang",	
				"fei",	"fen",	"feng",	"fo",	"fou",	"fu",	"ga",	"gai",	"gan",	"gang",	
				"gao",	"ge",	"gei",	"gen",	"geng",	"gong",	"gou",	"gu",	"gua",	"guai",	
				"guan",	"guang","gui",	"gun",	"guo",	"ha",	"hai",	"han",	"hang",	"hao",	
				"he",	"hei",	"hen",	"heng",	"hong",	"hou",	"hu",	"hua",	"huai",	"huan",	
				"huang","hui",	"hun",	"huo",	"ji",	"jia",	"jian",	"jiang","jiao",	"jie",	
				"jin",	"jing",	"jiong","jiu",	"ju",	"juan",	"jue",	"jun",	"ka",	"kai",	
				"kan",	"kang",	"kao",	"ke",	"ken",	"keng",	"kong",	"kou",	"ku",	"kua",	
				"kuai",	"kuan",	"kuang","kui",	"kun",	"kuo",	"la",	"lai",	"lan",	"lang",	
				"lao",	"le",	"lei",	"leng",	"li",	"lia",	"lian",	"liang","liao",	"lie",	
				"lin",	"ling",	"liu",	"long",	"lou",	"lu",	"lv",	"luan",	"lue",	"lun",	
				"luo",	"ma",	"mai",	"man",	"mang",	"mao",	"me",	"mei",	"men",	"meng",	
				"mi",	"mian",	"miao",	"mie",	"min",	"ming",	"miu",	"mo",	"mou",	"mu",	
				"na",	"nai",	"nan",	"nang",	"nao",	"ne",	"nei",	"nen",	"neng",	"ni",	
				"nian",	"niang","niao",	"nie",	"nin",	"ning",	"niu",	"nong",	"nu",	"nv",	
				"nuan",	"nue",	"nuo",	"o",	"ou",	"pa",	"pai",	"pan",	"pang",	"pao",	
				"pei",	"pen",	"peng",	"pi",	"pian",	"piao",	"pie",	"pin",	"ping",	"po",	
				"pu",	"qi",	"qia",	"qian",	"qiang","qiao",	"qie",	"qin",	"qing",	"qiong",	
				"qiu",	"qu",	"quan",	"que",	"qun",	"ran",	"rang",	"rao",	"re",	"ren",	
				"reng",	"ri",	"rong",	"rou",	"ru",	"ruan",	"rui",	"run",	"ruo",	"sa",	
				"sai",	"san",	"sang",	"sao",	"se",	"sen",	"seng",	"sha",	"shai",	"shan",	
				"shang","shao",	"she",	"shen",	"sheng","shi",	"shou",	"shu",	"shua",	"shuai",	
				"shuan","shuang","shui","shun",	"shuo",	"si",	"song",	"sou",	"su",	"suan",	
				"sui",	"sun",	"suo",	"ta",	"tai",	"tan",	"tang",	"tao",	"te",	"teng",	
				"ti",	"tian",	"tiao",	"tie",	"ting",	"tong",	"tou",	"tu",	"tuan",	"tui",	
				"tun",	"tuo",	"wa",	"wai",	"wan",	"wang",	"wei",	"wen",	"weng",	"wo",	
				"wu",	"xi",	"xia",	"xian",	"xiang","xiao",	"xie",	"xin",	"xing",	"xiong",	
				"xiu",	"xu",	"xuan",	"xue",	"xun",	"ya",	"yan",	"yang",	"yao",	"ye",	
				"yi",	"yin",	"ying",	"yo",	"yong",	"you",	"yu",	"yuan",	"yue",	"yun",	
				"za",	"zai",	"zan",	"zang",	"zao",	"ze",	"zei",	"zen",	"zeng",	"zha",	
				"zhai",	"zhan",	"zhang","zhao",	"zhe",	"zhen",	"zheng","zhi",	"zhong","zhou",	
				"zhu",	"zhua",	"zhuai","zhuan","zhuang","zhui","zhun",	"zhuo",	"zi",	"zong",	
				"zou",	"zu",	"zuan",	"zui",	"zun",	"zuo"};
        #endregion

        /// <summary>
        /// 获取汉字的汉语拼音
        /// </summary>
        /// <param name="source">欲转换的字符串</param>
        /// <param name="spellOptions">SpellOptions枚举值的按位 OR 组合</param>
        /// <example>
        /// <code>
        /// string source = "一只棕色狐狸跳过那只狗";
        /// string result = StringHelper.ConvertChsToPinYin(source, HZSpellOption.EnableUnicodeLetter);
        /// </code>
        /// </example>
        /// <returns>转换之后的结果</returns>
        public static string ConvertChsToPinYin(string source, HZSpellOption spellOptions)
        {
            StringBuilder builder = new StringBuilder(128);
            if (source.Length > 0)
            {
                char[] sourceChars = source.ToCharArray();

                for (int i = 0; i < sourceChars.Length; i++)
                {
                    byte[] byteArray = Encoding.GetEncoding("GB2312").GetBytes(sourceChars[i].ToString());

                    if (byteArray[0] <= 128)//汉字库之外的字符，半角字符
                    {
                        if ((spellOptions & HZSpellOption.EnableUnicodeLetter) == HZSpellOption.EnableUnicodeLetter)
                            builder.Append(sourceChars[i]);
                    }
                    else
                    {
                        //string temp = string.Empty;
                        switch ((int)byteArray[0])
                        {
                            //参见 http://ash.jp/code/cn/gb2312tbl.htm 中的GB2312字库分析结果
                            case 0xA3://全角 ASCII
                                if ((spellOptions & HZSpellOption.EnableUnicodeLetter) == HZSpellOption.EnableUnicodeLetter)
                                    builder.Append(sourceChars[i]);
                                break;
                            case 0xA1://特殊字符：± × ÷ ∶ ∧ ∨
                            case 0xA2://罗马数字：⒈ ⒉ ⒊以及Ⅰ Ⅱ Ⅲ Ⅳ Ⅴ等
                            case 0xA4://类似韩文：ぁ あ ぃ い ぅ
                            case 0xA5://类似日文：ゲ コ ゴ サ 
                            case 0xA6://希腊字母：а б в г
                            case 0xA7://??字母：а б в г
                            case 0xA8://??字母：à ē é
                            case 0xA9://框架字符：┢ ┣ ┤ ┥
                                if ((spellOptions & HZSpellOption.EnableUnknownWord) == HZSpellOption.EnableUnknownWord)
                                    builder.Append(sourceChars[i]);
                                break;
                            case 0xAA:
                            case 0xAB:
                            case 0xAC:
                            case 0xAD:
                            case 0xAE:
                            case 0xAF:
                            case 0xF8:
                            case 0xF9:
                            case 0xFA:
                            case 0xFB:
                            case 0xFC:
                            case 0xFD:
                            case 0xFE:
                            case 0xFF:
                                ExceptionHelper.TrueThrow(true, "非法字符“{0}”", sourceChars[i].ToString());
                                break;
                            default://汉字
                                int ascii = (short)(byteArray[0]) * 256 + (short)(byteArray[1]) - 65536;

                                bool added = false;
                                for (int j = pyvalue.Length - 1; j >= 0; j--)
                                {
                                    if (pyvalue[j] <= ascii)
                                    {
                                        string s = pystr[j];
                                        if ((spellOptions & HZSpellOption.FirstLetterUpper) == HZSpellOption.FirstLetterUpper)
                                            s = s[0].ToString().ToUpper() + s.Substring(1, s.Length - 1);

                                        if ((spellOptions & HZSpellOption.FirstLetterOnly) == HZSpellOption.FirstLetterOnly)
                                            builder.Append(s[0]);
                                        else
                                            builder.Append(s);

                                        added = true;
                                        break;
                                    }
                                }

                                if (false == added && (spellOptions & HZSpellOption.EnableUnknownWord) == HZSpellOption.EnableUnknownWord)
                                    builder.Append(sourceChars[i]);

                                break;
                        }
                    }

                }
            }

            return builder.ToString();
        }

        #endregion

        #region 常用正则表达式实现
        /// <summary>
        /// 是否存在正则表达式的匹配成功项目
        /// </summary>
        /// <param name="pattern">正则表达式的定义</param>
        /// <param name="input">待匹配的字符创</param>
        /// <returns>匹配是否成功</returns>
        /// <remarks>是否存在正则表达式的匹配成功项目。该函数的目的在于简化正则表达式的构造过程</remarks>
        public static bool HasPattern(string pattern, string input)
        {
            Regex regEx = new Regex(pattern);
            return regEx.IsMatch(input);
        }

        /// <summary>
        /// 检查字符串是否合法 Email 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法Email格式</remarks>
        public static bool IsValidEmail(this string source)
        {
            return HasPattern(Properties.RegExpResource.EMail, source);
        }
        /// <summary>
        /// 检查字符串是否合法 Interger[整数] 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 Interger[整数] 格式</remarks>
        public static bool IsValidInterger(this string source)
        {
            return HasPattern(Properties.RegExpResource.Interger, source);
        }

        /// <summary>
        /// 检查字符串是否合法 金额【最多两位小树】 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 金额【最多两位小数】 格式</remarks>
        public static bool IsValidMoney(this string source)
        {
            return HasPattern(Properties.RegExpResource.Money, source);
        }

        /// <summary>
        /// 检查字符串是否合法 全字母【不分大小写】 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 全字母【不分大小写】 格式</remarks>
        public static bool IsValidAlpha(this string source)
        {
            return HasPattern(Properties.RegExpResource.OnlyAlpha, source);
        }

        /// <summary>
        /// 检查字符串是否合法 字母【不分大小写】和数字 格式
        /// </summary>
        /// <param name="source"></param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 字母【不分大小写】和数字 格式</remarks>
        public static bool IsValidAlphaAndNumber(this string source)
        {
            return HasPattern(Properties.RegExpResource.OnlyAlphaAndNumber, source);
        }

        /// <summary>
        /// 检查字符串是否合法 大些字母 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 大些字母 格式</remarks>
        public static bool IsValidAlphaUpper(this string source)
        {
            return HasPattern(Properties.RegExpResource.OnlyAlphaUpper, source);
        }
        /// <summary>
        /// 检查字符串是否合法 小写字母 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 小写字母 格式</remarks>
        public static bool IsValidAlphaLower(this string source)
        {
            return HasPattern(Properties.RegExpResource.OnlyAlphaLower, source);
        }
        /// <summary>
        /// 检查字符串是否合法 数字串 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 数字串 格式</remarks>
        public static bool IsValidNumber(this string source)
        {
            return HasPattern(Properties.RegExpResource.OnlyNumber, source);
        }
        /// <summary>
        /// 检查字符串是否合法 汉字字符串 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 汉字字符串 格式</remarks>
        public static bool IsValidChinese(this string source)
        {
            return HasPattern(Properties.RegExpResource.OnlyChinese, source);
        }
        /// <summary>
        /// 检查字符串是否合法 Url地址【Ftp、Http等】 格式
        /// </summary>
        /// <param name="source">待检查的字符串</param>
        /// <returns>是否合法</returns>
        /// <remarks>检查字符串是否合法 Url地址【Ftp、Http等】 格式</remarks>
        public static bool IsValidUrl(this string source)
        {
            return HasPattern(Properties.RegExpResource.Url, source);
        }

        /// <summary>
        /// 是不是IP地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidIPAddress(this string str)
        {
            IPAddress ip = null;
            bool result = false;

            if (str.IsNotEmpty())
                result = IPAddress.TryParse(str, out ip);

            return result;
        }
        #endregion

        /*
		#region 字符编码处理  added by wangxiang May 28, 2008

		/// <summary>
		/// 将ASCII byte[]变成string
		/// </summary>
		/// <param name="source">byte[]</param>
		/// <returns>string</returns>
		public static string ASCIIBytesToString(byte[] source)
		{
			ASCIIEncoding asciiEncoder = new ASCIIEncoding();
			return asciiEncoder.GetString(source);
		}

		/// <summary>
		/// string 变成 ASCII byte[]
		/// </summary>
		/// <param name="source">string</param>
		/// <returns>byte[]</returns>
		public static byte[] ASCIIStringToBytes(string source)
		{
			ASCIIEncoding asciiEncoder = new ASCIIEncoding();
			return asciiEncoder.GetBytes(source);
		}

		/// <summary>
		/// 将Base64编码字符串转换为byte[]
		/// </summary>
		/// <param name="source">string</param>
		/// <returns>byte[]</returns>
		public static byte[] FromBase64String(string source)
		{
			return Convert.FromBase64String(source);
		}

		/// <summary>
		/// 将byte[]转换为Base64编码字符串
		/// </summary>
		/// <param name="source">byte[]</param>
		/// <returns>string</returns>
		public static string ToBase64String(byte[] source)
		{
			return Convert.ToBase64String(source);
		}

		/// <summary>
		/// 将unicode byte[]转换为字符串
		/// </summary>
		/// <param name="source">byte[]</param>
		/// <returns>string</returns>
		public static string UnicodeBytesToString(byte[] source)
		{
			UnicodeEncoding unicodeEncoder = new UnicodeEncoding();
			return unicodeEncoder.GetString(source);
		}

		/// <summary>
		/// 将Unicode字符串转换为 byte[]
		/// </summary>
		/// <param name="source">string</param>
		/// <returns>byte[]</returns>
		public static byte[] UnicodeStringToBytes(string source)
		{
			UnicodeEncoding unicodeEncoder = new UnicodeEncoding();
			return unicodeEncoder.GetBytes(source);
		}

		/// <summary>
		/// 将byte[]转换为Base16字符串
		/// </summary>
		/// <param name="source">byte[]</param>
		/// <returns>string</returns>
		public static string ToBase16String(byte[] source)
		{
			if ((source == null) || (source.Length == 0))
				return string.Empty;
			StringBuilder hexString = new StringBuilder();
			for (int i = 0; i < source.Length; i++)
			{
				hexString.Append(string.Format("{0:X2}", source[i]));
			}

			return hexString.ToString();
		}

		/// <summary>
		/// 将Base16字符串转换为byte[]
		/// </summary>
		/// <param name="source">string</param>
		/// <returns>byte[]</returns>
		public static byte[] FromBase16String(string source)
		{
			if (string.IsNullOrEmpty(source))
				return null;
			if (source.Length % 2 != 0)
				throw new ArgumentException("source");

			int length = (int)(source.Length / 2);
			byte[] hexByteArray = new byte[length];
			for (int i = 0; i < length; i++)
				hexByteArray[i] = byte.Parse(
					source.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);

			return hexByteArray;
		}

		#endregion
		*/
    }
}
