﻿using ZKWeb.Database;
using ZKWeb.Localize;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Demo.Plugins.ZKWeb.Demo.src.Components.EntityOperationHandlers {
	/// <summary>
	/// 防止编辑和删除演示用的账号
	/// </summary>
	[ExportMany]
	public class PreventDeleteSelf : IEntityOperationHandler<User> {
		public void AfterDelete(IDatabaseContext context, User data) { }
		public void AfterSave(IDatabaseContext context, User data) { }

		public void BeforeSave(IDatabaseContext context, User data) {
			if (data.Username == "test" || data.Username == "demo") {
				throw new ForbiddenException(new T("Edit or delete demo account is not allowed"));
			}
		}

		public void BeforeDelete(IDatabaseContext context, User data) {
			BeforeSave(context, data);
		}
	}
}
