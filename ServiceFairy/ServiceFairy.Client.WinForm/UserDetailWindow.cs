using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Client.WinForm
{
    public partial class UserDetailWindow : Common.WinForm.Docking.DockContentEx
    {
        public UserDetailWindow(Contact contact, UserContext userCtx)
        {
            _userCtx = userCtx;
            InitializeComponent();

            Update(contact);
            Text = "详情:" + contact.UserName;
        }

        private Contact _contact;
        private readonly UserContext _userCtx;

        public void Update(Contact contact)
        {
            _contact = contact;
            _firstNameTextBox.Text = contact.FirstName;
            _lastNameTextBox.Text = contact.LastName;
        }
    }
}
