namespace Project.DAL.AppMetaData
{
    public static class Router
    {
        public const string Root = "Api";
        public const string Version = "v1";
        public const string Rule = Root + "/" + Version + "/";

        public static class CategoryRouting
        {
            public const string Prefix = Rule + "Categories/";
            public const string List = Prefix + "List";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit";
            public const string Delete = Prefix + "Delete";

        }

        public static class BrandRouting
        {
            public const string Prefix = Rule + "Brand/";
            public const string List = Prefix + "List";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit";
            public const string Delete = Prefix + "Delete";

        }
        public static class ProductRouting
        {
            public const string Prefix = Rule + "Products/";
            public const string List = Prefix + "List";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit";
            public const string Delete = Prefix + "Delete";
            public const string Pagination = Prefix + "Pagination";

        }

        public static class AccountRouting
        {
            public const string Prefix = Rule + "Account/users/";
            public const string Register = Prefix + "Register";
            public const string List = Prefix + "List";
            public const string changePassword = Prefix + "changePassword";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit";
            public const string Delete = Prefix + "Delete";
            public const string Pagination = Prefix + "Pagination";

        }
        public static class AuthenticationRouting
        {
            public const string Prefix = Rule + "Authentication/";
            public const string SignIn = Prefix + "SignIn";
            public const string RefreshToken = Prefix + "RefreshToken";
            public const string SendCodeResetPassword = Prefix + "SendCodeResetPassword";
            public const string ResetPassword = Prefix + "ResetPassword";
            public const string ConfirmResetPassword = Prefix + "ConfirmResetPassword";
            public const string ConfirmEmail = "/Api/Authentication/ConfirmEmail";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit";
            public const string Delete = Prefix + "Delete";
            public const string Pagination = Prefix + "Pagination";

        }

        public static class AuthorizationRouting
        {
            public const string Prefix = Rule + "Authorization/";
            public const string AddRole = Prefix + "AddRole";
            public const string ManageUserRole = Prefix + "ManageUserRole";
            public const string GetAllRoles = Prefix + "GetAllRoles";
            public const string UpdateRole = Prefix + "UpdateRole";
            public const string UpdateUserRole = Prefix + "UpdateUserRole";
            public const string DeleteRole = Prefix + "DeleteRole";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string GetById = Prefix + "GetByName/{id:int}";
            public const string Pagination = Prefix + "Pagination";

        }


    }
}
