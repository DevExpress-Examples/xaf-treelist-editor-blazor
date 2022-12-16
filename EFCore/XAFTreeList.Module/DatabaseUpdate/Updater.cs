﻿using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using XAFTreeList.Module.BusinessObjects;

namespace XAFTreeList.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            var projectGroup = ObjectSpace.FindObject<ProjectGroup>(null);
            if(projectGroup == null) {
                projectGroup = ObjectSpace.CreateObject<ProjectGroup>();
                projectGroup.Name = "ASP.NET AJAX Controls";
                var project = ObjectSpace.CreateObject<Project>();
                project.Name = "Navigation and Layout";
                var projectArea = ObjectSpace.CreateObject<ProjectArea>();
                projectArea.Name = "ASPxMenu";
                projectArea.Project = project;
                project.ProjectGroup = projectGroup;

                projectGroup = ObjectSpace.CreateObject<ProjectGroup>();
                projectGroup.Name = "WinForms Controls";
                project = ObjectSpace.CreateObject<Project>();
                project.Name = "Grid and Data Editors for WinForms";
                projectArea = ObjectSpace.CreateObject<ProjectArea>();
                projectArea.Name = "XtraGrid Suite";
                projectArea.Project = project;
                project.ProjectGroup = projectGroup;
            }
            ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
        }
    }
}