using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using MCS.Web.WebControls;

namespace MCS.Web.WebControls.Test 
{
    [Serializable]   
    public class DeluxeGridPropertiesCls
    {
        private int dataSourceMaxRow = 0;
        private string dataSourceType = "";
        private Object setObjectDataSource;
        private string gridTitle = "";
        private  Color titleColor;
        private bool pagerExportMode;
        private bool iDataSource = false;
        private string exportCommandArgument = "";
        private bool checkBoxAdd;
        private RowPosition checkBoxPosition;
        private RowPosition exportPosition;

		/// <summary>
		/// �Ƿ��ѡ
		/// </summary>
		public bool MultiSelect
		{
			get;
			set;
		}

        /// <summary>
        /// ָ��GridView�ϵ������ݵ�����Դ���������
        /// </summary> 
        public int DataSourceMaxRow
        {
            get { return this.dataSourceMaxRow; }
            set { this.dataSourceMaxRow = value; }
        }

        /// <summary>
        /// ָ��GridView�ϵ������ݵ�����Դ����
        /// </summary> 
        public string DataSourceType
        {
            get
            {
                return this.dataSourceType ?? "";
            }
            set { this.dataSourceType = value; }
        }
 
        public Object SetObjectDataSource
        {
            get
            {
                return this.setObjectDataSource ?? null;
            }
            set { this.setObjectDataSource = value; }
        }


        /// <summary>
        /// ָ��GridView����ʾ��������
        /// </summary> 
        public string GridTitle
        {
            get { return this.gridTitle ?? "����"; }
            set { this.gridTitle = value; }
        }

        /// <summary>
        /// ����������ɫ
        /// </summary> 
        public Color TitleColor
        {
            get { return ParseColor(this.titleColor, Color.FromArgb(141, 143, 149)); }
            set { this.titleColor = value; }
        }

        //[Browsable(true),
        //Category("��չ"),
        //DefaultValue(FontUnit.Large),
        //Description("��������")]
        //public  TitleFont
        //{
        //    get { return (FontUnit)(ViewState["TitleFont"] ?? FontUnit.Large); }
        //    set { ViewState["TitleFont"] = value; }
        //}

        /// <summary>
        /// �����Ƿ���ʾ��������
        /// </summary> 
        public bool PagerExportMode
        {
            get
            {
                object o = this.pagerExportMode;
                return o == null ? false : (bool)o;
            }
            set { this.pagerExportMode = value; }
        }
        /// <summary>
        /// ���õ���Text
        /// </summary> 
        public bool IDataSource
        {
            get
            {
                return this.iDataSource ;
            }
            set { this.iDataSource = value; }
        }

        /// <summary>
        /// ��������
        /// </summary> 
        public string ExportCommandArgument
        {
            get
            {
                return this.exportCommandArgument ?? string.Empty;
            }
            set { this.exportCommandArgument = value; }
        }


        /// <summary>
        /// �����Ƿ�����checkbox��
        /// </summary> 
        public bool CheckBoxAdd
        {
            get
            {
                object o = this.checkBoxAdd;
                return o == null ? false : (bool)o;
            }
            set { this.checkBoxAdd = value; }
        }

        /// <summary>
        /// ����checkbox�е�λ��
        /// </summary>  
        public RowPosition CheckBoxPosition
        {
            get { return ParseRowPosition(this.checkBoxPosition); }
            set { this.checkBoxPosition = value; }
        }

        /// <summary>
        /// ���õ����е�λ��
        /// </summary>  
        public RowPosition ExportPosition
        {
            get { return ParseRowPosition(this.exportPosition); }
            set { this.exportPosition = value; }
        }
 
        public static RowPosition ParseRowPosition(object o)
        {
            return ParseRowPosition(o, new RowPosition());
        }

         
        public static RowPosition ParseRowPosition(object o, RowPosition defaultValue)
        {
            if (o == null || o.ToString() == "")
            {
                return defaultValue;
            }
            try
            {
                return (RowPosition)Enum.Parse(typeof(RowPosition), o.ToString(), true);
            }
            catch
            {
                throw new FormatException("'" + o.ToString() + "' ���͸ı�ʧ��");
            }
        }
        public static Color ParseColor(object o, Color defaultValue)
        {
            if (o == null || o.ToString() == "")
            {
                return defaultValue;
            }
            try
            {
                return ColorTranslator.FromHtml(o.ToString());
            }
            catch
            {
                throw new FormatException("'" + o.ToString() + "' can not be parsed as a color.");
            }
        }
    }
}
