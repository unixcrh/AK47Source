﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Web.UI.HtmlControls;

using MCS.Library.Core;
using MCS.Library.SOA.DataObjects;
using MCS.Web.Library.Resources;
using MCS.Web.Library.Script;

[assembly: WebResource("MCS.Web.WebControls.PropertyForm.PropertyForm.js", "application/x-javascript")]
[assembly: WebResource("MCS.Web.WebControls.PropertyForm.PropertyForm.css", "text/css", PerformSubstitution = true)]

namespace MCS.Web.WebControls
{
	[PersistChildren(false)]
	[ParseChildren(true)]
	[RequiredScript(typeof(ControlBaseScript), 1)]
	[RequiredScript(typeof(ClientMsgResources), 2)]
	[RequiredScript(typeof(ClientPropertyEditorControlBaseResources), 3)]
	[ClientCssResource("MCS.Web.WebControls.PropertyForm.PropertyForm.css")]
	[ClientScriptResource("MCS.Web.WebControls.PropertyForm", "MCS.Web.WebControls.PropertyForm.PropertyForm.js")]
	public class PropertyForm : PropertyEditorControlBase
	{
		private PropertyLayoutCollection _Layouts = new PropertyLayoutCollection();

		public PropertyForm()
			: base()
		{
			JSONSerializerExecute.RegisterConverter(typeof(PropertyLayoutConverter));
		}

		[PersistenceMode(PersistenceMode.InnerProperty), Description("布局属性定义")]
		[MergableProperty(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue((string)null)]
		[Browsable(false)]
		[ScriptControlProperty, ClientPropertyName("formsections")]
		public PropertyLayoutCollection Layouts
		{
			get
			{
				return this._Layouts;
			}
		}

		protected override void LoadClientState(string clientState)
		{
			object[] state = JSONSerializerExecute.Deserialize<object[]>(clientState);

			if (state[0] != null)
				this.Properties.CopyFrom(JSONSerializerExecute.Deserialize<PropertyValueCollection>(state[0]));

			if (state[1] != null)
				this._Layouts.CopyFrom(JSONSerializerExecute.Deserialize<PropertyLayoutCollection>(state[1]));

		}
	}
}
