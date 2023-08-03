using System;
using System.Collections.Generic;

public class Admin
{
    public Admin()


    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
}

public class AdminRepository
{
    private List<Admin> admins = new List<Admin>();
    private int nextId = 1;

    public Admin CreateAdmin(string username, string password)
    {
        Admin newAdmin = new Admin
        {
            Id = nextId++,
            Username = username,
            Password = password
        };

        admins.Add(newAdmin);
        return newAdmin;
    }

    public Admin GetAdminById(int id)
    {
        return admins.Find(admin => admin.Id == id);
    }

    public List<Admin> GetAllAdmins()
    {
        return admins;
    }

    public void UpdateAdmin(Admin updatedAdmin)
    {
        Admin existingAdmin = admins.Find(admin => admin.Id == updatedAdmin.Id);
        if (existingAdmin != null)
        {
            existingAdmin.Username = updatedAdmin.Username;
            existingAdmin.Password = updatedAdmin.Password;
        }
    }

    public void DeleteAdmin(int id)
    {
        Admin adminToRemove = admins.Find(admin => admin.Id == id);
        if (adminToRemove != null)
        {
            admins.Remove(adminToRemove);
        }
    }
}

public class AdminController
{
    private AdminRepository adminRepository;

    public AdminController()
    {
        adminRepository = new AdminRepository();
    }

    public Admin CreateAdmin(string username, string password)
    {
        return adminRepository.CreateAdmin(username, password);
    }

    public Admin GetAdminById(int id)
    {
        return adminRepository.GetAdminById(id);
    }

    public List<Admin> GetAllAdmins()
    {
        return adminRepository.GetAllAdmins();
    }

    public void UpdateAdmin(Admin updatedAdmin)
    {
        adminRepository.UpdateAdmin(updatedAdmin);
    }

    public void DeleteAdmin(int id)
    {
        adminRepository.DeleteAdmin(id);
    }
}

class Program
{
    static void Main(string[] args)
    {
        AdminController adminController = new AdminController();

      
        Admin newAdmin = adminController.CreateAdmin("admin1", "password1");
        Console.WriteLine("Created admin: " + newAdmin.Username);

        Admin retrievedAdmin = adminController.GetAdminById(newAdmin.Id);
        Console.WriteLine("Retrieved admin: " + retrievedAdmin.Username);

        List<Admin> allAdmins = adminController.GetAllAdmins();
        Console.WriteLine("All admins:");
        foreach (Admin admin in allAdmins)
        {
            Console.WriteLine(admin.Username);
        }

        newAdmin.Password = "newpassword";
        adminController.UpdateAdmin(newAdmin);
        Console.WriteLine("Updated admin password");

        adminController.DeleteAdmin(newAdmin.Id);
        Console.WriteLine("Deleted admin");

        Console.ReadLine();
    }
}
