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
    public partial class Top_Menu : UserControl
    {

        public delegate void MouseOver_EventHandler(string _data);
        public event MouseOver_EventHandler Mouse_Over;


        public delegate void Mouseclick_EventHandler(string _data);
        public event Mouseclick_EventHandler Mouse_click;


        //public delegate void Mouse_Lever_EventHandler(string _data);
        //public event Mouse_Lever_EventHandler Mouse_Lever;


        public Top_Menu()
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
          
        }

        private void Top_Menu_MouseLeave(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(0, 75, 143);
           // Title.Font = new System.Drawing.Font(Title.Font.Name, 10F);
            Title.ForeColor = Color.White;


            if (Mouse_Over != null)
            {
                Mouse_Over(Title.Text+":"+"out");
            }

        }

        private void Top_Menu_MouseEnter(object sender, EventArgs e)
        {
          //  this.BackColor = Color.FromArgb(241, 241, 241);
         //   Title.Font = new System.Drawing.Font(Title.Font.Name, 12F);
            Title.ForeColor = Color.DarkOrange;

            if (Mouse_Over != null)
            {
                Mouse_Over(Title.Text + ":" + "in");
            }
        }

        private void Title_Click(object sender, EventArgs e)
        {
             if (Mouse_click != null)
             {
                  Mouse_click(Title.Text);
             }
        }

        private void Top_Menu_Click(object sender, EventArgs e)
        {
             if (Mouse_click != null)
             {
                  Mouse_click(Title.Text);
             }
        }


    }
}
