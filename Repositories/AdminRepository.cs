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
