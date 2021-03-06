// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Reflection;

namespace MCS.Web.Library.Script
{
    /// <summary>
	/// 表明客户端控件属性的名称，通过此属性自定义服务端控件属性对应到客户端控件属性的名称
    /// Allows the mapping of a property declared in managed code to a property
    /// declared in client script.  For example, if the client script property is named "handle" and you
    /// prefer the name on the TargetProperties object to be "Handle", you would apply this attribute with the value "handle."
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ClientPropertyNameAttribute : Attribute
    {
        private string _propertyName;

		/// <summary>
		/// 构造函数
		/// </summary>
        public ClientPropertyNameAttribute()
        {
        }

        /// <summary>
        /// Creates an instance of the ClientPropertyNameAttribute and initializes
        /// the PropertyName value.
        /// </summary>
        /// <param name="propertyName">The name of the property in client script that you wish to map to.</param>
        public ClientPropertyNameAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        /// <summary>
		/// 对应的客户端属性名称
        /// The name of the property in client script code that you wish to map to.
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

		/// <summary>
		/// 检查属性上，是否有ClientPropertyName属性，如果有，则使用它的PropertyName，否则使用Property自己的Name
		/// </summary>
		/// <param name="pi"></param>
		/// <returns></returns>
		public static string GetClientPropertyName(PropertyInfo pi)
		{
			ClientPropertyNameAttribute nameAttr = (ClientPropertyNameAttribute)Attribute.GetCustomAttribute(pi, typeof(ClientPropertyNameAttribute), false);
			
			return nameAttr == null ? pi.Name : nameAttr.PropertyName;
		}
    }
}
