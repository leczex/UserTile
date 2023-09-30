using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace UserTile.Properties
{
	// Token: 0x02000005 RID: 5
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002E0A File Offset: 0x0000100A
		internal Resources()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002E18 File Offset: 0x00001018
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("UserTile.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002E64 File Offset: 0x00001064
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002E7B File Offset: 0x0000107B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002E84 File Offset: 0x00001084
		internal static Bitmap userpic
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("userpic", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x0400001B RID: 27
		private static ResourceManager resourceMan;

		// Token: 0x0400001C RID: 28
		private static CultureInfo resourceCulture;
	}
}
