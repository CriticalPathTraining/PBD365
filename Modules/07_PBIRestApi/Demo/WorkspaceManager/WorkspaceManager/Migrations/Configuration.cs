using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace WorkspaceManager.Migrations {

  internal sealed class Configuration : DbMigrationsConfiguration<WorkspaceManager.Models.ApplicationDbContext> {

    public Configuration() {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(WorkspaceManager.Models.ApplicationDbContext context) {}
  }
}
