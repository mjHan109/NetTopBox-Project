using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fs_32
{
    public partial class sub_Menu : UserControl
    {

         public delegate void Mouseclick_EventHandler(string _data);
         public event Mouseclick_EventHandler Mouse_click;


        public sub_Menu()
        {
            InitializeComponent();
        }


        [Category("Data")]
        [Description("test")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string set_title
        {
            get
            {
                return this.Title.Text;
            }
            set
            {
                this.Title.Text = value;
            }
        }


        [Category("Data")]
        [Description("test")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool set_left_bar
        {
            get
            {
                if ( Left_bar.Visible == true)
                {
                  return  true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    Left_bar.Visible = true;
                }
                else
                {
                    Left_bar.Visible = false;
                }
            }
        }

        [Category("Data")]
        [Description("test")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool set_Right_bar
        {
            get
            {
                if (right_bar.Visible == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    right_bar.Visible = true;
                }
                else
                {
                    right_bar.Visible = false;
                }
            }
        }

        [Category("Data")]
        [Description("test")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always),Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool set_bottom_bar
        {
            get
            {
                if (bottom_bar.Visible == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    bottom_bar.Visible = true;
                }
                else
                {
                    bottom_bar.Visible = false;
                }
            }
        }

        private void Top_Menu_MouseUp(object sender, MouseEventArgs e)
        {
           // 
          
        }

        private void Top_Menu_MouseLeave(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(0, 75, 143);
           // Title.Font = new System.Drawing.Font(Title.Font.Name, 10F);
            Title.ForeColor = Color.Black;
        }

        private void Top_Menu_MouseEnter(object sender, EventArgs e)
        {
          //  this.BackColor = Color.FromArgb(241, 241, 241);
         //   Title.Font = new System.Drawing.Font(Title.Font.Name, 12F);
            Title.ForeColor = Color.OrangeRed;
        }

        private void Title_Click(object sender, EventArgs e)
        {
             if (Mouse_click != null)
             {
                  Mouse_click(Title.Text);
             }
        }

        private void sub_Menu_Click(object sender, EventArgs e)
        {
             if (Mouse_click != null)
             {
                  Mouse_click(Title.Text);
             }
        }

        private void bottom_bar_Click(object sender, EventArgs e)
        {
             if (Mouse_click != null)
             {
                  Mouse_click(Title.Text);
             }
        }


    }
}
