using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MCS.Web.WebControls.Test
{
    public class Tools
    {
        public static byte[] StringToByteArray(string str)
        {
            if (str != null)
                return System.Text.Encoding.Default.GetBytes(str);
            else
                return null;
        }
        public static string ByteArrayToStr(byte[] ByteArray)
        {
            string ret = "";
            if (ByteArray != null && ByteArray.Length > 0)
            {
                ret = System.Text.Encoding.Default.GetString(ByteArray);
                //IndexOf0 = ret.IndexOf('\0');
                //if (IndexOf0 >= 0 && ret.Length > 0)
                //    ret = ret.Substring(0, IndexOf0).Trim();
            }
            return ret;
        }
        ///     <summary>   
         ///     ���л�Ϊ�������ֽ�����   
         ///     </summary>   
         ///     <param     name="request">Ҫ���л��Ķ���</param>   
         ///     <returns>�ֽ�����</returns>   
         public static byte[] SerializeBinary(object request)
         {
             BinaryFormatter serializer = new BinaryFormatter();
             MemoryStream memStream = new MemoryStream();
             serializer.Serialize(memStream, request);
             return memStream.GetBuffer();
         }

        
         ///     <summary>   
         ///     �Ӷ��������鷴���л��õ�����   
         ///     </summary>   
         ///     <param     name="buf">�ֽ�����</param>   
         ///     <returns>�õ��Ķ���</returns>        
         public static object DeserializeBinary(byte[] buf)
         {
             System.IO.MemoryStream ms = new MemoryStream();
              
             BinaryFormatter deserializer = new BinaryFormatter();
             ms.Write(buf, 0, buf.Length); 
             ms.Seek(0, SeekOrigin.Begin); 
             object newobj = deserializer.Deserialize(ms);
             
             ms.Close();
             return newobj;        
         } 

        NameValueCollection extendedAttributes = new NameValueCollection();
        public string DeluxePagerString
        {
            get { return GetExtendedAttribute("DeluxePager"); }
            set { SetExtendedAttribute("DeluxePager", value); }
        }
        // ��NameValueCollection������ȡ��¼ 
        public string GetExtendedAttribute(string name)
        {
            string returnValue = extendedAttributes[name];

            if (returnValue == null)
                return string.Empty;
            else
                return returnValue;
        }

        // ������չ���Ե���NameValueCollection�еļ�ֵ��ֵ 
        public void SetExtendedAttribute(string name, string value)
        {
            extendedAttributes[name] = value;
        }

        // ��extendedAttributes����ǰ�涨��������������е��û���չ��Ϣ��NameValueCollection�������л�Ϊ�ڴ��� 
        // �����������浽���ݿ��� 
        public byte[] SerializeExtendedAttributes()
        {

            // ���л����� 
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            // ����һ���ڴ��������л��󱣴������� 
            MemoryStream ms = new MemoryStream();
            byte[] b;

            // ��extendedAttributes�������汣�������е��û���չ��Ϣ�����л�Ϊ�ڴ��� 
            // 
            binaryFormatter.Serialize(ms, extendedAttributes);

            // �����ڴ�������ʼλ�� 
            // 
            ms.Position = 0;

            // ���뵽 byte ���� 
            // 
            b = new Byte[ms.Length];
            ms.Read(b, 0, b.Length);
            ms.Close();

            return b;
        }

        // �����л�extendedAttributes��������� 
        // �����ݿ��ж�ȡ������ 
        public void DeserializeExtendedAttributes(byte[] serializedExtendedAttributes)
        {

            if (serializedExtendedAttributes.Length == 0)
                return;
            try
            {

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                ms.Seek(0, SeekOrigin.Begin); 
                // �� byte ���鵽�ڴ��� 
                // 
                ms.Write(serializedExtendedAttributes, 0, serializedExtendedAttributes.Length);

                // ���ڴ�����λ�õ��ʼλ�� 
                // 
                ms.Position = 0;

                // �����л���NameValueCollection���󣬴�������ԭ������ȫ��ͬ�ĸ��� 
                // 
                extendedAttributes = (NameValueCollection)binaryFormatter.Deserialize(ms);

                ms.Close();
            }
            catch { }

        }

        public static void GetDeluxePager(MCS.Web.WebControls.DeluxePager DeluxePager1, string pagerObj, ref PagerPropertiesCls ppc)
        { 
            byte[] b = new byte[] { };

            if (pagerObj != null)
            {
                string[] strPager = pagerObj.Split(';');
                b = new Byte[strPager.Length];
                for (int i = 0; i < strPager.Length; i++)
                {
                    b[i] = Convert.ToByte(strPager[i]);
                }  
                object obj = Tools.DeserializeBinary(b);
                if (obj != null)
                {
                    ppc = (PagerPropertiesCls)obj;
                    //GetRequest(ppc);
                    ppc.InitializeDeluxePager(DeluxePager1, ppc);
                }
            } 
        }

        public static PagerPropertiesCls GetPagerProperties(string pagerObj)
        {
            PagerPropertiesCls ppc = new PagerPropertiesCls();
            byte[] b = new byte[] { };

            if (pagerObj != null)
            {
                string[] strPager = pagerObj.Split(';');
                b = new Byte[strPager.Length];
                for (int i = 0; i < strPager.Length; i++)
                {
                    b[i] = Convert.ToByte(strPager[i]);
                }
                object obj = Tools.DeserializeBinary(b);
                if (obj != null)
                {
                    ppc = (PagerPropertiesCls)obj; 
                }
            } 
            return ppc;
        }

        public static void SetClientPagerProperties(MCS.Web.WebControls.DeluxePager DeluxePager1, PagerPropertiesCls ppc)
        {
            if (ppc != null)
            {
                ppc.InitializeDeluxePager(DeluxePager1, ppc);
            }
        }
    }
    public class DataAccess
    {
        private const string ConnectionString = "ConnectionString";

        private DataAccess()
        {
        }

        public static DataView GetOrders(int count)
        {            
            return ObjData.RunSqlReturnDS(string.Format("SELECT TOP {0} * FROM ORDERS", count)).Tables[0].DefaultView; 
        }

        public static DataView GetProductsOfOrders(string orderID)
        {
            DataTable table = new DataTable("ORDERS_PRODUCTS");

            if (string.IsNullOrEmpty(orderID) == false)
            {
                string sql = string.Format("SELECT * FROM ORDERS_PRODUCTS WHERE ORDER_ID = '{0}'", orderID.Replace("\'", "\'\'"));
                table = ObjData.RunSqlReturnDS(sql).Tables[0]; 
            } 
            return table.DefaultView;
        }

        public static int GetOrdersCount()
        { 
            return (int)ObjData.RunSqlReturnObject("SELECT COUNT(*) FROM ORDERS");
        }

        public static string GetCustomerFullNameByShortName(string shortName)
        {
            return ObjData.RunSqlReturnObject("SELECT CUSTOMER_NAME FROM CUSTOMER_TEMPLATE WHERE SHORT_NAME = '" + shortName + "'").ToString();             
        }
    }
}
