using System;


	public IAdminRepository()
	{

        public interface IAdminRepository
    {
        Admin CreateAdmin(string username, string password);
        Admin GetAdminById(int id);
        List<Admin> GetAllAdmins();
        void UpdateAdmin(Admin updatedAdmin);
        void DeleteAdmin(int id);
    }

}
}
