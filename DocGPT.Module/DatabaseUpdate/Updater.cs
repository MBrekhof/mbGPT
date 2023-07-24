﻿using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.EF;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DocGPT.Module.BusinessObjects;
using DevExpress.ExpressApp.SystemModule;

namespace DocGPT.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        ApplicationUser userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "Admin");
        if (userAdmin == null)
        {
            userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
            userAdmin.UserName = "Admin";
            // Set a password if the standard authentication type is used
            userAdmin.SetPassword("");

            /* The UserLoginInfo object requires a user object Id (Oid).
               Commit the user object to the database before you create a UserLoginInfo object.
               This will correctly initialize the user key property. */

            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication,
            ObjectSpace.GetKeyValueAsString(userAdmin));
        }
        // If a role with the Administrators name doesn't exist in the database, create this role.
        PermissionPolicyRole adminRole =
        ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if (adminRole == null)
        {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
        }
        //Set the user's role to Administrative. This role has access to objects of all types.
        adminRole.IsAdministrative = true;
        userAdmin.Roles.Add(adminRole);
        ApplicationUser commonUser = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "User");
        if (commonUser == null)
        {
            commonUser = ObjectSpace.CreateObject<ApplicationUser>();
            commonUser.UserName = "User";
            // Set a password if the standard authentication type is used
            commonUser.SetPassword("");

            /* The UserLoginInfo object requires a user object Id (Oid).
               Commit the user object to the database before you create a UserLoginInfo object.
               This will correctly initialize the user key property.*/

            ObjectSpace.CommitChanges(); //This line persists created object(s).

            ((ISecurityUserWithLoginInfo)commonUser).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication,
            ObjectSpace.GetKeyValueAsString(commonUser));
        }
        PermissionPolicyRole defaultRole = CreateDefaultRole();
        commonUser.Roles.Add(defaultRole);
        ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
    }
    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
    }
    private PermissionPolicyRole CreateDefaultRole()
    {
        PermissionPolicyRole defaultRole =
        ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if (defaultRole == null)
        {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read,
            cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails",
            SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write,
            "ChangePasswordOnFirstLogon", cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(),
            SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write,
            "StoredPassword", cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(),
            SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read,
            SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess,
            SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess,
            SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create,
            SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create,
            SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
}
