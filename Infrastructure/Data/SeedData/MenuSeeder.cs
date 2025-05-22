using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.SeedData;

public class MenuSeeder(ApplicationDbContext context) : ISeeder
{
        public void Seed()
    {
        if (!context.Menus.Any())
        {
            var dashboard = new Menu
            {
                Name = "Bảng điều khiển",
                Url = "/dashboard",
                Description = "Trang tổng quan",
                Icon = "layout-dashboard",
                IsBlank = false,
                Order = 1
            };

            var science = new Menu
            {
                Name = "Nhiệm vụ khoa học",
                Url = "#",
                Description = "Quản lý nhiệm vụ khoa học",
                Icon = "file-text",
                IsBlank = false,
                Order = 2
            };

            var taskNew = new Menu
            {
                Name = "Nhiệm vụ mới",
                Url = "/tasks/new",
                Description = "Thêm nhiệm vụ khoa học mới",
                Icon = "plus-square",
                IsBlank = false,
                Order = 1,
                Parent = science
            };

            var taskList = new Menu
            {
                Name = "Danh sách nhiệm vụ",
                Url = "/tasks",
                Description = "Danh sách nhiệm vụ đã tạo",
                Icon = "list",
                IsBlank = false,
                Order = 2,
                Parent = science
            };

            var taskStat = new Menu
            {
                Name = "Thống kê",
                Url = "/tasks/statistics",
                Description = "Thống kê theo lĩnh vực, đơn vị",
                Icon = "bar-chart-3",
                IsBlank = false,
                Order = 3,
                Parent = science
            };

            var system = new Menu
            {
                Name = "Quản lý hệ thống",
                Url = "#",
                Description = "Hệ thống và người dùng",
                Icon = "settings",
                IsBlank = false,
                Order = 3
            };

            var user = new Menu
            {
                Name = "Người dùng",
                Url = "/system/users",
                Description = "Quản lý tài khoản người dùng",
                Icon = "user",
                IsBlank = false,
                Order = 1,
                Parent = system
            };

            var permission = new Menu
            {
                Name = "Quyền",
                Url = "/system/permissions",
                Description = "Quản lý quyền truy cập",
                Icon = "shield",
                IsBlank = false,
                Order = 2,
                Parent = system
            };

            var menu = new Menu
            {
                Name = "Menu",
                Url = "/system/menus",
                Description = "Cấu hình menu",
                Icon = "list-tree",
                IsBlank = false,
                Order = 3,
                Parent = system
            };

            var role = new Menu
            {
                Name = "Vai trò",
                Url = "#",
                Description = "Phân quyền người dùng",
                Icon = "users-cog",
                IsBlank = false,
                Order = 4,
                Parent = system
            };

            var roleList = new Menu
            {
                Name = "Danh sách vai trò",
                Url = "/system/roles",
                Description = "Tất cả vai trò",
                Icon = "users",
                IsBlank = false,
                Order = 1,
                Parent = role
            };

            var rolePerm = new Menu
            {
                Name = "Cấu hình quyền",
                Url = "/system/roles/permissions",
                Description = "Gán quyền cho vai trò",
                Icon = "key",
                IsBlank = false,
                Order = 2,
                Parent = role
            };

            var roleMenu = new Menu
            {
                Name = "Cấu hình menu",
                Url = "/system/roles/menus",
                Description = "Phân menu cho vai trò",
                Icon = "sliders-horizontal",
                IsBlank = false,
                Order = 3,
                Parent = role
            };

            var category = new Menu
            {
                Name = "Quản lý danh mục",
                Url = "#",
                Description = "Danh mục dùng chung",
                Icon = "database",
                IsBlank = false,
                Order = 4
            };

            var unit = new Menu
            {
                Name = "Đơn vị",
                Url = "/categories/departments",
                Description = "Danh mục đơn vị",
                Icon = "building",
                IsBlank = false,
                Order = 1,
                Parent = category
            };

            var status = new Menu
            {
                Name = "Trạng Thái",
                Url = "/categories/statuses",
                Description = "Trạng thái nhiệm vụ",
                Icon = "circle-dot",
                IsBlank = false,
                Order = 2,
                Parent = category
            };

            var field = new Menu
            {
                Name = "Lĩnh vực",
                Url = "/categories/fields",
                Description = "Lĩnh vực nghiên cứu",
                Icon = "book",
                IsBlank = false,
                Order = 3,
                Parent = category
            };

            context.Menus.AddRange(
                dashboard,
                science, taskNew, taskList, taskStat,
                system, user, permission, menu, role, roleList, rolePerm, roleMenu,
                category, unit, status, field
            );
            context.SaveChanges();
        }
    }
}