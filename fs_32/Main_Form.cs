using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using System.Collections;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using System.Runtime.InteropServices;
using fs_32.CFT_Module.Model;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;
using fs_32.MyThread;
using System.Text.RegularExpressions;
using fs_32.Function_Page;
using System.Diagnostics;
using fs_32.Menu;
using System.Security.Cryptography;
using System.Security.Principal;
using fs_32.UTIL;
using Microsoft.Win32;
using System.Net;
using System.Net.NetworkInformation;
using DevExpress.XtraEditors;
using Encoding = System.Text.Encoding;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Security.AccessControl;
using System.Management;
using IronPython.Runtime.Operations;

namespace fs_32
{
    public partial class Main_Form : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        #region 20211118 김준수 수정
        //private CFT_Module.FileSystemBuilder fsBuilder = null;
        #endregion

        public ArrayList drvlist;
        private Dictionary<long, DisplayObject> Tot_File_Info = new Dictionary<long, DisplayObject>(); //20210706 검색된 모든 파일 저장 변수


        public bool search_copy_flag = false;
        public string Working_Step = "";
        private fs_32.Menu.Three.MenuThree_base _three_base = null;


        public File_Wrie_Controller _fc = null;

        private fs_32.Function_Page.Fillter_Setting _fs = null;

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 2000 };  // 20230628 progress.txt 파일을 위한 timer

        private bool chk_foot_cancel = false;

        private bool chk_indexing = false;

        public bool setEnable = true;

        public Main_Form()
        {
            InitializeComponent();

            #region 20230628 한민정 추가
            // progress.txt 파일을 위한 timer
            timer.Tick += OnTimerTick;
            timer.Start();
            #endregion

            NTS_Common_Property._pPoint = this;


            //  top_Menu1.Mouse_Over += new fs_32.Top_Menu.MouseOver_EventHandler(mo_c);
            //  top_Menu1.Mouse_click += new fs_32.Top_Menu.Mouseclick_EventHandler(m_click);


            top_Menu2.Mouse_Over += new fs_32.Top_Menu.MouseOver_EventHandler(mo_c);
            top_Menu2.Mouse_click += new fs_32.Top_Menu.Mouseclick_EventHandler(m_click); // PC 상세분석


            top_Menu3.Mouse_Over += new fs_32.Top_Menu.MouseOver_EventHandler(mo_c);
            top_Menu3.Mouse_click += new fs_32.Top_Menu.Mouseclick_EventHandler(m_click); // 목록작성

            top_Menu4.Mouse_Over += new fs_32.Top_Menu.MouseOver_EventHandler(mo_c);
            top_Menu5.Mouse_Over += new fs_32.Top_Menu.MouseOver_EventHandler(mo_c);
            top_Menu6.Mouse_Over += new fs_32.Top_Menu.MouseOver_EventHandler(mo_c);

            top_Menu7.Mouse_Over += new fs_32.Top_Menu.MouseOver_EventHandler(mo_c);
            top_Menu7.Mouse_click += new fs_32.Top_Menu.Mouseclick_EventHandler(m_click);

            sub_Menu12.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);
            sub_Menu7.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click); // 특정폴더 이미징

            //  top_Menu8.Mouse_click += new fs_32.Top_Menu.Mouseclick_EventHandler(m_click); // 필터설정

            // 20230711 한민정 추가 인덱싱 버튼 추가
            top_Menu8.Mouse_Over += new fs_32.Top_Menu.MouseOver_EventHandler(mo_c);
            top_Menu8.Mouse_click += new fs_32.Top_Menu.Mouseclick_EventHandler(m_click);

            sub_Menu16.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);
            sub_Menu15.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);
            sub_Menu14.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);
            sub_Menu13.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);

            sub_Menu11.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);
            sub_Menu8.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);


            sub_Menu1.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);
            sub_Menu2.Mouse_click += new fs_32.sub_Menu.Mouseclick_EventHandler(ms_click);





            //sub1.Visible = false;
            sub2.Visible = false;
            sub3.Visible = false;
            sub4.Visible = false;



            //  fs_32.Menu.Two.Menu_Base _one = new fs_32.Menu.Two.Menu_Base();
            Fillter_Setting _one = new Fillter_Setting();
            _fs = _one;
            set_contests(_one);


            cb_position.Properties.Items.Add("직원");
            cb_position.Properties.Items.Add("대리");
            cb_position.Properties.Items.Add("팀장");
            cb_position.Properties.Items.Add("과장");
            cb_position.Properties.Items.Add("부장");
            cb_position.Properties.Items.Add("상무");
            cb_position.Properties.Items.Add("전무");
            cb_position.Properties.Items.Add("이사");
            cb_position.Properties.Items.Add("대표이사");

            txt_pcuser.Text = (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1];
            txt_company.Text = SystemInformation.ComputerName;

            // 20230628 한민정 서버 전송 페이지에 default 서버 값
            //NTS_Common_Property.server_ip = "192.168.239.138";
            //NTS_Common_Property.server_port = "9999";

            NTS_Common_Property.server_ip = "127.0.0.1";
            NTS_Common_Property.server_port = "12345";



            label1.Text = Config.main_title;





        }


        public void init_data()
        {
            treeList1.Nodes.Clear();


            if (NTS_Common_Property.search_mode == 1)
            {
                drive_list();
                NTS_Common_Property.search_mode = 1;
                NTS_Common_Property.local_copy = true;

                working_state.Text = "(일반모드)";
                //    chk_file_server_upload.Checked = false;
                //    Left_Top_Base.Height = 372;
                del_file_get.Checked = false;
                del_file_get.Visible = false;
            }
            else
            {

                try
                {
                    CFT_Driver_info();
                    working_state.Text = "(고급모드)";
                    NTS_Common_Property.search_mode = 0;
                    //    chk_file_server_upload.Checked = false;

                    NTS_Common_Property.local_copy = true;

                    //      Left_Top_Base.Height = 266;

                    drive_list_Net();

                    del_file_get.Checked = true;
                    del_file_get.Visible = true;



                }
                catch (Exception ex)
                {
                    drive_list();
                    NTS_Common_Property.search_mode = 1;
                    working_state.Text = "(일반모드)";
                    //    chk_file_server_upload.Checked = false;
                    //   Left_Top_Base.Height = 372;
                    del_file_get.Checked = false;
                    del_file_get.Visible = false;
                    NTS_Common_Property.local_copy = true;
                }
            }





            treeList1.ExpandAll();

        }

        public void init_fillter()
        {


            NTS_Common_Property.DTF_NEW.Rows.Clear();
            NTS_Common_Property.DTF_NEW.Columns.Clear();
            NTS_Common_Property.DTF_NEW.Columns.Add("종류");
            NTS_Common_Property.DTF_NEW.Columns.Add("필터");

            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "회계", "ACCDB;MDB;SDB;DDF;DBS;NSZ;DBF;DZI;DZW;FBK;FDB;ORA" }); // LOG 2021.10.16 김수용반장님 요청 삭제 //DB
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "기타문서", "CSV;TSV;GUL;HST;KEY;PAGES;NUMBERS;SNT;sqlite;PDF;XPS;HWX;HWN;WPD" }); //TXT;RTF;
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "메일", "EML;EMLX;MSG;DBX;MBOX;OST;PST;NSF" });
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "한컴문서", "HWP;HWPX;NXL;SHOW;ODS;CELL" });
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "엑셀", "XLA;XLAM;XLB;XLC;XLK;XLM;XLS;XLSB;XLSM;XLSX;XLT;XLTM;XLTX;XLW" });
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "파워포인트", "ODP;POT;POTX;PPA;PPAM;PPS;PPSM;PPSX;PPT;PPTM;PPTX" });
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "워드", "DOC;DOCM;DOCX;DOT;DOTX;ODT;ONE;WBK;WPS" });
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "클라우드문서", "NDOC;NXLS;NPPT;GDOC;GDOCX;GSHEET;GSLIDES;SGV" });
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "압축", "ZIP;7z;ARJ;EGG;ALZ;RAR;TAR;GZ" });
            NTS_Common_Property.DTF_NEW.Rows.Add(new object[] { "이미지", "GIF;JPG;JPEG;BMP;PNG;RLE;PSD;PDD;TIF;RAW;HEIC" });



            NTS_Common_Property.NTS_File_Fillter.Clear();
            int row_count = 0;
            foreach (DataRow row in NTS_Common_Property.DTF_NEW.Rows)
            {
                row_count++;
                // 20230706 한민정 수정 - 파일 확보가 zip 수집하는지 여부 확인 후 수정
                //if (row_count > 8) break; // 클라우드 까지만 default

                string[] _data = row["필터"].ToString().Split(';');

                for (int i = 0; i < _data.Length; i++)
                {

                    NTS_Common_Property.NTS_File_Fillter.Add(_data[i].Trim().ToUpper(), _data[i].Trim().ToUpper());
                }


            }




        }


        #region 사용안함
        /*
          private CFT_Module.FileSystemBuilder fsBuilder = null;

          public string get_save_path()
          {

               // "^" 를 하는 이유는 경우에 따라 뒤쪽 부분만 필요한 경우가 있어서
               return "";



          }

          public ArrayList drvlist;

          private void button1_Click(object sender, EventArgs e)
          {
               fsBuilder = new CFT_Module.FileSystemBuilder();
               drvlist = fsBuilder.GetDrivesInfo_step1(); ;
               TreeListNode root = null;
               TreeListNode itemnode = null;
               string[] node = null;

               // PhysicalDrive
               //[0] = {| \\.\PhysicalDrive0 | 70 GB |  | VMware Virtual NVMe Disk | 0000_0000_0000_0000. |  | Physical | 0 | 0 | }
               //[1] = {| \\.\C: | 59 GB | Fixed |  | 3cce-2801 | NTFS | Logical | 1159168 | 0 | }
               //Console.WriteLine(drvlist);

               string _ims = "";

               foreach (var item in drvlist)
               {

                    node = item.ToString().Split('|');

                    _ims = node[1].Replace(@"\.\", "").ToString();
                    _ims = _ims.Replace(@"\", "").Trim();

                    if (node[1].IndexOf("PhysicalDrive") > -1)
                    {
                         root = treeList1.AppendNode(new object[] { _ims + "(Size:" + node[2] + ")" + node[4], node[1] }, null);

                    }
                    else
                    {

                         treeList1.AppendNode(new object[] { _ims + "(Size:" + node[2] + ")" + node[6], node[1] }, root);
                    }

               }

          }
          */
        #endregion

        private fs_32.Menu.Start_info _stinfo = null;


        public DateTime Start_time; // 작업진행 시간 계산용


        private void Files_Get_Start_Click(object sender, EventArgs e)
        {

            //if (NTS_Common_Property._pPoint.process_start) 
            //{
            //     NTS_Common_Property._pPoint.show_alert("진행 정보", "파일 확보 작업이 진행중 입니다.");
            //     return;
            //}


            start_button_init_off();

            _stinfo = null;

            string copy_folder = "";
            copy_folder = txt_company.Text.Trim();
            copy_folder += "_" + txt_depart.Text.Trim();
            copy_folder += "_" + cb_position.Text.Trim();
            copy_folder += "_" + txt_pcuser.Text.Trim();

            NTS_Common_Property.continue_copy_chk = false;
            NTS_Common_Property.copy_position = copy_path.Text + @"\" + copy_folder; // copy_position 저장위치가 없으면 현재위치 아니면 설정 경로

            string sdisklist = "";
            int dev_count = 0;

            Start_time = DateTime.Now;
            f_total = 0;

            if (NTS_Common_Property.search_mode == 0)
            {
                #region  고급모드 국보연 모듈에 선택한 드라이브 정보 확인
                treeList1.ExpandAll();

                for (int i = 0; i < treeList1.Nodes.TreeList.AllNodesCount; i++)
                {
                    TreeListNode ff = treeList1.GetNodeByVisibleIndex(i);

                    try
                    {
                        if (ff.GetValue("드라이브2").ToString().IndexOf(@"\.\") > -1)
                        {
                            if (ff.Checked == true)
                            {
                                sdisklist += dev_count + "^";

                            }
                        }

                        dev_count++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("일반모드는 드라이브2 내용이 없어서");
                    }


                }

                ///////////////////////////////////////////////////////////

                #region 네트워크 드라입 진행 확인 주석 처리

                if (sdisklist.Equals(""))
                {

                    NTS_Common_Property.search_mode = 1;
                    // init_data();

                    search_drive_list();

                    if (sdrive_list.Count == 0)
                    {

                        show_alert("사용자 오류", "선택한 드라이브가 없습니다." + Environment.NewLine + "검색할 드라이브를 선택해주세요.");
                        start_button_init_on();



                        return;
                    }


                    #region 230628 한민정 사용 안하는 이전 주석처리 된 코드
                    ////show_alert("사용자 오류", "선택한 드라이브가 없습니다." + Environment.NewLine + "검색할 드라이브를 선택해주세요.");
                    ////start_button_init_on();
                    ////return;

                    //show_alertYN("사용자 오류", "선택한 드라이브가 없습니다." + Environment.NewLine + "네트워크 플더나 특정 폴더를 검색 하시겠습니까?");

                    //if (NTS_Common_Property.Alert_Form_YN_plag)
                    //{
                    //     NTS_Common_Property.search_mode = 1;
                    //    // init_data();

                    //     search_drive_list();

                    //     if (sdrive_list.Count == 0)
                    //     {

                    //          show_alert("사용자 오류", "선택한 드라이브가 없습니다." + Environment.NewLine + "검색할 드라이브를 선택해주세요.");
                    //          start_button_init_on();



                    //          return;
                    //     }



                    //}
                    //else
                    //{
                    //     start_button_init_on();
                    //     return;
                    //}
                    #endregion
                }

                #endregion 선택 드라이브 체크


                #endregion 선택 드라이브 체크 확인 국보연 제공 한것둥에 확인 종료

            }
            else
            {
                #region 일반모드 드라이브 선택 확인

                search_drive_list();

                if (sdrive_list.Count == 0)
                {

                    show_alert("사용자 오류", "선택한 드라이브가 없습니다." + Environment.NewLine + "검색할 드라이브를 선택해주세요.");
                    start_button_init_on();



                    return;
                }

                #endregion 일반모드 드라이브 선택 확인 종료
            }





            #region 일반모드의 서버 전송 가능 여부 확인

            if (!NTS_Common_Property.local_copy)
            {
                // 일반모드의 서버확보경우 오류 확인
                #region 서버 전송 오류 확인
                int iNum = Sever_Folder_Check();

                if (iNum == 2)
                {
                    show_alert("시스템 오류", "전송할 위치에 동일한 업체 및 사용자명이 존재합니다." + Environment.NewLine + "업체 및 사용자명 변경 후 다시 실행하세요!");

                    start_button_init_on();

                    return;
                }
                else if (iNum == 1)
                {

                    show_alert("시스템 오류", "확보 서버에 접속 할 수 없습니다." + Environment.NewLine + "서버 확인 후 다시 실행하세요.");
                    start_button_init_on();

                    return;
                }
                #endregion 서버 전송 오류 확인 종료

            }
            #endregion 일반모드의 서버 전송 가능 여부 확인 종료

            //이어서 하기 진행      
            NTS_Common_Property.continue_copy_chk = false;
            FileInfo _file_info = new FileInfo(NTS_Common_Property.copy_position + @"\5.일반모드_확보_리스트.txt");
            if (_file_info.Exists)
            {
                show_alertYN("사용자 오류", "일반모드의 파일리스트 정보가 있습니다." + Environment.NewLine + "확보내역 진행하시겠습니까?");

                if (NTS_Common_Property.Alert_Form_YN_plag)
                {
                    if (NTS_Common_Property.search_mode == 0)
                    {
                        show_alert("진행정보", "고급모드는 이어하기를 지원하지 않습니다." + Environment.NewLine + "확보 위치를 변경후 다시 진행해 주세요.");
                        start_button_init_on();
                        return;
                    }
                    else
                    {
                        NTS_Common_Property.continue_copy_chk = true;
                        NTS_Common_Property.search_mode = 1;
                        Save_file_normal_mode_local();
                        return;
                    }

                }
                else
                {
                    start_button_init_on();
                    return;
                }

            }




            // 저장 폴더 생성
            if (!System.IO.Directory.Exists(NTS_Common_Property.copy_position))
            {
                System.IO.Directory.CreateDirectory(NTS_Common_Property.copy_position);
            }




            _fc = new File_Wrie_Controller(NTS_Common_Property.copy_position);

            _stinfo = new fs_32.Menu.Start_info();
            _stinfo.set_1("");
            _stinfo.set_2("");
            set_contests(_stinfo); // 시작 정보 화면에 출력

            if (NTS_Common_Property.logical_imageing == false)
            {

                if (NTS_Common_Property.search_mode == 0) // search_mode 0: 고급 모드 1: 일반모드
                {

                    Files_Get_Start_advenced(sdisklist);

                }
                else
                {
                    Files_Get_Start_normal();

                }
            }
            else
            {

                /////////////////////////////////////////////////// 이미징 처리 구간 ////////////////////////////////////////////

                Console.WriteLine("이미징 시작");


                dev_count = 0;

                //이미징은 지정한 경로가 없으면 파일이 생성이 안된다   

                DirectoryInfo dd = new DirectoryInfo(NTS_Common_Property.copy_position);
                if (!dd.Exists)
                {
                    dd.Create();
                }





                #region 20211118 김준수 수정
                // fsBuilder = new CFT_Module.FileSystemBuilder();
                CFT_Module.FileSystemBuilder fsBuilder = CFT_Module.FileSystemBuilder.GetInstance();
                #endregion

                fsBuilder.CFT_imaging_Process_Persent_send += new CFT_Module.FileSystemBuilder.CFT_imaging_Process_Persent_EventHandler(progressBarControl1_Setting);

                Thread th = new Thread(() =>
                {
                    //선택한 드라이브 정보를 한번에 넘겨서 처리한다.
                    fsBuilder.Folder_imageing_extends_start(drvlist, sdisklist, ref Tot_File_Info);

                });

                th.IsBackground = true;    // 메인 종료시 같이 종료

                th.Start();
            }




        }




        #region 화면처리

        //파일복사진행단계
        public delegate void setdata0(string _p);
        private void mo_c(string _p)
        {

            if (top_Base.InvokeRequired)
            {

                setdata0 dr = new setdata0(mo_c);
                this.Invoke(dr, new object[] { _p });
            }
            else
            {
                if (_p.Split(':')[1].Equals("in"))
                {
                    change_submenu(_p);
                }

            }


        }

        private void change_submenu(string _val)
        {
            string[] _dat = _val.Split(':');
            set_inti();
            switch (_dat[0])
            {
                case "PC 원클릭 점검":
                    // top_Base.Height = 80; 

                    //   top_Menu1.set_bottom_bar = true;
                    // sub1.Visible = false;
                    sub2.Visible = false;
                    sub3.Visible = false;
                    sub4.Visible = false;
                    break;
                case "PC 상세분석":
                    //  top_Base.Height = 117;
                    top_Menu2.set_bottom_bar = true;
                    //  sub1.Visible = true;
                    sub2.Visible = false;
                    sub3.Visible = false;
                    sub4.Visible = false;
                    break;
                case "목록작성":
                    //  top_Base.Height = 80;
                    top_Menu3.set_bottom_bar = true;
                    //  sub1.Visible = false;
                    sub2.Visible = false;
                    sub3.Visible = false;
                    sub4.Visible = false;
                    break;
                case "옵션설정":
                    //  top_Base.Height = 117;
                    top_Menu4.set_bottom_bar = true;
                    //   sub1.Visible = false;
                    sub2.Visible = true;
                    sub3.Visible = false;
                    sub4.Visible = false;
                    break;
                case "이미징":
                    //   top_Base.Height = 117;
                    top_Menu5.set_bottom_bar = true;
                    //   sub1.Visible = false;
                    sub2.Visible = false;
                    sub3.Visible = true;
                    sub4.Visible = false;
                    break;
                case "TOOL":
                    //  top_Base.Height = 117;
                    top_Menu6.set_bottom_bar = true;
                    //   sub1.Visible = false;
                    sub2.Visible = false;
                    sub3.Visible = false;
                    sub4.Visible = true;
                    break;
                case "풋프린팅":
                    //  top_Base.Height = 117;
                    top_Menu7.set_bottom_bar = true;
                    //  sub1.Visible = false;
                    sub2.Visible = false;
                    sub3.Visible = false;
                    sub4.Visible = false;
                    break;
                case "인덱싱":
                    top_Menu8.set_bottom_bar = false;
                    sub2.Visible = false;
                    sub3.Visible = false;
                    sub4.Visible = false;
                    break;

            }
        }

        private void set_inti()
        {
            //    top_Menu1.set_bottom_bar = false;
            top_Menu2.set_bottom_bar = false;
            top_Menu3.set_bottom_bar = false;
            top_Menu4.set_bottom_bar = false;
            top_Menu5.set_bottom_bar = false;
            top_Menu6.set_bottom_bar = false;
            top_Menu7.set_bottom_bar = false;
            top_Menu8.set_bottom_bar = false;
        }

        public void set_contests(UserControl _x1)
        {


            Right_Menu_items.Controls.Clear();
            Right_Menu_items.Controls.Add(_x1);
            _x1.Dock = DockStyle.Fill;


        }

        public bool process_start = false; // 작업진영 여부

        private void start_button_init_on()
        {
            process_start = false;

            Files_Get_Start.Enabled = true;
            set_save_position_Button.Enabled = true;

            treeList1.Enabled = true;
            panel2.Enabled = true;
            //  File_Server_info_base.Enabled = true;
            del_file_get.Enabled = true;


            //  NTS_Common_Property._pPoint.process_start = false;

        }

        private void start_button_init_off()
        {

            process_start = true;

            Files_Get_Start.Enabled = false;
            set_save_position_Button.Enabled = false;
            treeList1.Enabled = false;
            panel2.Enabled = false;
            //  File_Server_info_base.Enabled = false;
            del_file_get.Enabled = false;
            //    NTS_Common_Property._pPoint.process_start = true;
        }

        #region  화면 이동 및 종료 최소화 처리

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private Point mouseoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mouseoint.X = e.X), this.Top - (mouseoint.Y - e.Y));
            }
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.min_over;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.min;
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.close3_o;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.close3;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //  Application.ExitThread();
            Environment.Exit(0);
            //  Application.DoEvents();

        }

        #endregion

        private void Files_Get_Start_MouseEnter(object sender, EventArgs e)
        {
            Files_Get_Start.Appearance.BackColor = Color.Red;
        }
        private void Files_Get_Start_MouseLeave(object sender, EventArgs e)
        {
            Files_Get_Start.Appearance.BackColor = Color.FromArgb(0, 75, 143);
        }

        private void set_save_position_Button_MouseEnter(object sender, EventArgs e)
        {
            set_save_position_Button.Appearance.BackColor = Color.Red;
        }

        private void set_save_position_Button_MouseLeave(object sender, EventArgs e)
        {
            set_save_position_Button.Appearance.BackColor = Color.FromName("GradientInactiveCaption");
        }



        private Pre_check_result P_chk;

        public delegate void setdata1(string _p);
        private void m_click(string _p)
        {
            if (top_Base.InvokeRequired)
            {

                setdata1 dr = new setdata1(m_click);
                this.Invoke(dr, new object[] { _p });
            }
            else
            {

                if (NTS_Common_Property.chk_imageing)
                {
                    show_alert("진행정보", "디스크 이미징 중 입니다.");
                    return;
                }


                if (_p.Equals("사전분석"))
                {
                    // NTS_Common_Property.License_key_chk = false;
                    // NTS_Common_Property.search_mode = 1;
                    //if (NTS_Common_Property.License_key_chk == false || NTS_Common_Property.search_mode == 1)
                    //{
                    // 2022.01.03 김수용반장 요청으로 삭제 show_alert("진행정보", "사전분석은 일반 모드에서는"+Environment.NewLine+"지원되지 않습니다.");
                    //     return;
                    //}

                    if (chk_foot_cancel)
                    {
                        show_alert("진행정보", "사전분석이 취소 되었습니다.");
                        top_Menu7.Enabled = false;
                        // 20230713 한민정 추가 사전분석 취소되면 인덱싱 버튼 Enable false로
                        top_Menu8.Enabled = false;
                        return;
                    }

                    if (P_chk != null)
                    {
                        set_contests(P_chk);
                    }

                    // 20230629 한민정 파일 표시 경로 수정
                    //FileInfo _im = new FileInfo(Application.StartupPath + @"\Footprinting\HTML_Export\FootPrinting.htm");
                    FileInfo _im = new FileInfo(Application.StartupPath + @"\FootprintingStandalone\Footprinting\HTML_Export\FootPrinting.htm");
                    if (_im.Exists)
                    {
                        // 20320704 한민정 수정
                        // 기존 코드에서는 Footprinting.htm 파일이 있으면 사전분석 클릭했을 때 보여줬지만
                        // 작성한 시간이 지금보다 이전이면 보여주지 않는 방식으로 변경
                        if (_im.LastWriteTime < DateTime.Now.AddMinutes(-1))
                        {
                            return;
                        }
                        //if (NTS_Common_Property._pPoint.process_start == true)
                        //{
                        //     show_alert("진행정보", "사전분석이 진행중 입니다" + Environment.NewLine + "분석이완료되면 자동으로 결과를 표시합니다.");
                        //     return;
                        //}
                        else
                        {
                            P_chk = new Pre_check_result();
                            set_contests(P_chk);
                        }

                        //  foorPprinting_result dd = new foorPprinting_result();
                        //   dd.foot_printing();
                        //   set_contests(dd);


                        //WebBrowser web1 = new WebBrowser();
                        //web1.ScriptErrorsSuppressed = true;

                        //web1.Url = new Uri(@"file://" + Application.StartupPath + @"\Footprinting\HTML_Export\FootPrinting.htm");

                        //panelControl2.Controls.Clear();
                        //panelControl2.Controls.Add(web1);

                        //web1.Dock = DockStyle.Fill;
                    }
                    else
                    {
                        //  MessageBox.Show("작업이 진행중 입니다.");
                        // show_alert("진행정보", "사전분석이 진행중 입니다" + Environment.NewLine + "분석이완료되면 자동으로 결과를 표시합니다.");

                        NTS_Common_Property._pPoint.show_alertYN("진행정보", "사전분석이 진행중입니다." + Environment.NewLine + "사전분석을 취소 하시겠습니까?.");

                        if (NTS_Common_Property.Alert_Form_YN_plag)
                        {
                            chk_foot_cancel = true;
                            NTS_Common_Property._pPoint.kill_foot_printing();
                        }


                    }

                }


                if (_p.Equals("PC 원클릭 점검"))
                {
                    set_contests(new fs_32.Menu.Two.Menu_Base());
                }

                if (_p.Equals("필터설정"))
                {
                    if (_fs == null)
                    {
                        _fs = new fs_32.Function_Page.Fillter_Setting();
                    }
                    set_contests(_fs);

                }

                if (_p.Equals("PC 상세분석"))
                {
                    if (_three_base == null)
                    {
                        _three_base = new fs_32.Menu.Three.MenuThree_base();
                    }
                    set_contests(_three_base);
                }


                if (_p.Equals("목록작성"))
                {

                    set_contests(new fs_32.Menu.Four.List_Create());
                }

                // 20230711 한민정 추가 인덱싱 버튼 관련
                if (_p.Equals("인덱싱"))
                {
                    if (chk_foot_cancel)
                    {
                        show_alert("진행정보", "사전분석 결과가 없습니다. 사전분석을 진행해주세요.");
                        top_Menu8.Enabled = false;
                        return;
                    }
                    indexing_start();
                    show_alert("진행정보", "인덱싱이 완료되었습니다");
                }
            }

        }

        private fs_32.Menu.Two.Menu_Base _etc_chk = null;

        public delegate void setdata2(string _p);
        private void ms_click(string _p)
        {
            if (top_Base.InvokeRequired)
            {

                setdata2 dr = new setdata2(ms_click);
                this.Invoke(dr, new object[] { _p });
            }
            else
            {


                if (NTS_Common_Property.chk_imageing)
                {
                    show_alert("진행정보", "디스크 이미징 중 입니다.");
                    return;
                }

                if (_p.Equals("네트워크 검색"))
                {
                    fs_32.Menu.Three.XForm_Net _ims = new fs_32.Menu.Three.XForm_Net();
                    _ims.Show();
                }

                if (_p.Equals("기타점검"))
                {
                    if (_etc_chk == null) _etc_chk = new fs_32.Menu.Two.Menu_Base();
                    set_contests(_etc_chk);
                }

                if (_p.Equals("고급옵션"))
                {
                    fs_32.Function_Page.Fillter_advanced_Setting _one = new fs_32.Function_Page.Fillter_advanced_Setting();
                    set_contests(_one);
                }

                if (_p.Equals("Logical"))
                {
                    if (NTS_Common_Property.search_mode == 0)
                    {

                        fs_32.Function_Page.logical_imaging _one = new fs_32.Function_Page.logical_imaging();
                        set_contests(_one);
                    }
                    else
                    {
                        show_alert("시스템 오류", "일반모드에서는 지원하지 않는 기능 입니다.");
                    }
                }

                if (_p.Equals("강제복사(복구)"))
                {
                    set_contests(new fs_32.Menu.Six.ForceCopy_new());
                }

                if (_p.Equals("분류복사"))
                {
                    set_contests(new fs_32.Menu.Six.file_concat());
                }

                if (_p.Equals("존재분석"))
                {
                    set_contests(new fs_32.Menu.Six.File_Exist());
                }

                if (_p.Equals("파일전송"))
                {
                    set_contests(new fs_32.Menu.Six.ServerCopy());
                }

                if (_p.Equals("수신서버설정"))
                {
                    CFP_Server.XForm_Server dd = new CFP_Server.XForm_Server();
                    dd.StartPosition = FormStartPosition.Manual;
                    dd.Location = new Point(this.Location.X + ((this.Width / 2) - (dd.Width / 2)), this.Location.Y + ((this.Height / 2) - (dd.Height / 2)));
                    dd.Show();

                }

                if (_p.Equals("DISK"))
                {
                    //MessageBox.Show("준비중...");
                    //  show_alert("시스템 정보", "준비중...");

                    if (NTS_Common_Property.search_mode == 0)
                    {

                        fs_32.Function_Page.Disk_imaging _one = new fs_32.Function_Page.Disk_imaging(this);
                        set_contests(_one);
                    }
                    else
                    {
                        show_alert("시스템 오류", "일반모드에서는 지원하지 않는 기능 입니다.");
                    }


                }

                //
            }


        }


        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Result_dir dd = new Result_dir();
            dd.StartPosition = FormStartPosition.Manual;
            dd.Location = new Point(this.Location.X + ((this.Width / 2) - (dd.Width / 2)), this.Location.Y + ((this.Height / 2) - (dd.Height / 2)));
            dd.ShowDialog();
        }



        #endregion




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region 국보연 모듈 구간

        private void CFT_Driver_info()
        {
            #region 20211118 김준수 수정
            CFT_Module.FileSystemBuilder fsBuilder = CFT_Module.FileSystemBuilder.GetInstance();

            KeyValuePair<ArrayList, Dictionary<short, Dictionary<ulong, DriveInfoSharp>>> drives = fsBuilder.GetDrivesInfo();
            drvlist = drives.Key;
            #endregion

            TreeListNode root = null;
            TreeListNode itemnode = null;
            string[] node = null;

            // PhysicalDrive
            //[0] = {| \\.\PhysicalDrive0 | 70 GB |  | VMware Virtual NVMe Disk | 0000_0000_0000_0000. |  | Physical | 0 | 0 | }
            //[1] = {| \\.\C: | 59 GB | Fixed |  | 3cce-2801 | NTFS | Logical | 1159168 | 0 | }
            //Console.WriteLine(drvlist);

            string _ims = "";

            string _pa = Application.StartupPath.Substring(0, 2);

            foreach (var item in drvlist)
            {

                node = item.ToString().Split('|');

                _ims = node[1].Replace(@"\.\", "").ToString();
                _ims = _ims.Replace(@"\", "").Trim();

                if (node[1].IndexOf("PhysicalDrive") > -1)
                {
                    root = treeList1.AppendNode(new object[] { _ims + "(Size:" + node[2] + ")" + node[4], node[1] }, null);

                }
                else
                {

                    treeList1.AppendNode(new object[] { _ims + "(Size:" + node[2] + ")" + node[6], node[1] }, root);
                }



            }

            //   treeList1.ExpandAll();

            string _at = Application.StartupPath.Substring(0, 2);

            for (int i = 0; i < treeList1.Nodes.Count; i++)
            {
                //  Console.WriteLine(treeList1.Nodes[i][1]);

                TreeListNode dd = treeList1.Nodes[i];

                bool _ck = false;
                for (int k = 0; k < dd.Nodes.Count; k++)
                {
                    _ims = dd.Nodes[k][1].ToString().Replace(@"\.\", "").ToString();
                    _ims = _ims.Replace(@"\", "").Trim();

                    // Console.WriteLine(dd.Nodes[k][1]);

                    if (_ims.Equals(_pa))
                    {
                        _ck = true;
                    }
                }

                if (!_ck) dd.CheckState = CheckState.Checked;






            }




        }



        ////// 국보연 모듈 실행 종료 확인
        public delegate void CFT_Drv_Process_end_EventHandler(string _data);
        public event CFT_Drv_Process_end_EventHandler Drv_Process_end;


        //국보연 파일 확보 
        private int Files_Get_Start_advenced(string sdisklist)
        {


            #region 국보연 모듈 실행


            Thread th = new Thread(() =>
            {

                // 선택한 드라이브가 여러개일 경우 BuildFS_StepByStep_step2 에서 모든 드라이브의  Tree 구조를 생성 한다 이부분이 시간이 좀걸려서
                // 사용자 ui 에 현재 진행 상태를 표시한다.
                #region 20211118 김준수 수정
                CFT_Module.FileSystemBuilder fsBuilder = CFT_Module.FileSystemBuilder.GetInstance();
                #endregion

                fsBuilder.CFT_Tree_Analysis_Process_Persent_send += new CFT_Module.FileSystemBuilder.CFT_Tree_Analysis_Process_Persent_EventHandler(progressBarControl1_Setting);
                fsBuilder.Drv_Process += new CFT_Module.FileSystemBuilder.CFT_Drv_Process_EventHandler(states_Message_setp_Setting);
                fsBuilder.File_Copy_Process += new CFT_Module.FileSystemBuilder.CFT_File_Copy_EventHandler(progressBarControl1_Setting);

                this.Drv_Process_end += new CFT_Drv_Process_end_EventHandler(cft_end);



                //선택한 드라이브 정보를 한번에 넘겨서 처리한다.
                #region 20211118 김준수 수정
                bool keepInHistory = true;  //파일시스템 분석 결과 저장 유무 설정 true:저장 O, false:저장 X
                ArrayList mediaList = fsBuilder.AnalyzeFileSystem(drvlist, sdisklist, keepInHistory);  //-> 드라이브별 
                #endregion

                #region 20211008 김준수 추가
                ///파일시스템 분석 후에 트리를 순회하며, 정상/삭제된 데이터만 추출해오는 부분
                ArrayList normalList = new ArrayList();
                ArrayList deletedList = null;

                if (del_file_get.Checked)
                {
                    deletedList = new ArrayList();
                }


                long file_total_size = 0; // 확보할 파일들의 전체 크기

                string _addtext = "";


                foreach (MediaSharp media in mediaList)
                {
                    int volumeSize = media.GetVolumeSize();

                    for (int i = 0; i < volumeSize; i++)
                    {
                        VolumeSharp volume = media.GetVolume(i);
                        #region 20211118 김준수 수정
                        ArrayList normalInVolume = fsBuilder.ExtractFiles(volume.GetRootFile(), CFT_Module.FileSystemBuilder.ExtractType.NORMAL_ONLY);//, ref  file_total_size);
                        #endregion

                        //  Console.WriteLine("일반파일 크기" + NTS_Common_Property.getSize(file_total_size));
                        //  file_total_size = 0;

                        normalList.AddRange(normalInVolume);
                        normalInVolume.Clear();

                        if (del_file_get.Checked)
                        {
                            #region 20211118 김준수 수정
                            ArrayList deletedInVolume = fsBuilder.ExtractFiles(volume.GetRootFile(), CFT_Module.FileSystemBuilder.ExtractType.DELETED_ONLY);//, ref file_total_size);
                            #endregion

                            //   Console.WriteLine("삭제파일 크기" + NTS_Common_Property.getSize(file_total_size));
                            deletedList.AddRange(deletedInVolume);
                            deletedInVolume.Clear();
                        }

                    }
                }

                #region 20211118 김준수 수정
                ////파일 사이즈 계산, normalList 전체에 대해서 구하면 되는지 확인 필요
                //file_total_size = fsBuilder.CalculateSize(normalList);


                // //2021.12.14
                // if (del_file_get.Checked) {
                //      file_total_size += file_total_size + fsBuilder.CalculateSize(deletedList);
                // }                        


                //DriveInfo df = new DriveInfo(NTS_Common_Property.copy_position.Substring(0, 1));


                //if (df.AvailableFreeSpace <= file_total_size)
                //{
                //    NTS_Common_Property.free_space_size = file_total_size - df.AvailableFreeSpace;
                //    Drv_Process_end("Free_space_not_enough");
                //    return;
                //};
                #endregion


                int normalAllCount = normalList.Count; // 디렉토리 포함
                int normalAllCount_real_file_count = 0; // 실제파일 숫자

                int deletedAllCount = 0;
                if (del_file_get.Checked)
                {
                    deletedAllCount = deletedList.Count; // 디렉토리 포함
                }

                int deletedAllCount_real_file_count = 0;  // 실제파일 숫자

                ArrayList normalFiles = new ArrayList();
                ArrayList deletedFiles = new ArrayList();


                string extension = "";
                //정상 데이터에서 파일만 추출, 디렉토리 제외됨
                foreach (FileSharp file in normalList)
                {
                    if (1 != file.GetNType())
                    {

                        if (NTS_Common_Property.fiilter_chk)
                        {
                            #region  필터적용

                            extension = file.GetExtension();

                            if ("" != extension && NTS_Common_Property.NTS_File_Fillter.ContainsKey(extension.ToUpper()))
                            {
                                if (NTS_Common_Property.search_advanced_option)
                                {
                                    //기간적용
                                    if (NTS_Common_Property.chk_date(file.get_creationTime(), file.get_accessTime(), file.get_modificationTime()))
                                    {
                                        normalFiles.Add(file);
                                        normalAllCount_real_file_count++;
                                    }
                                }
                                else
                                {
                                    //기간 미 적용
                                    normalFiles.Add(file);
                                    normalAllCount_real_file_count++;
                                }


                            }
                            else
                            {

                            }

                            #endregion 필터적용

                        }
                        else
                        {
                            #region 전체목록 확보

                            normalFiles.Add(file);
                            normalAllCount_real_file_count++;

                            #endregion
                        }

                    }
                }

                int normalFileCount = normalFiles.Count;

                //지워진 데이터에서 파일만 추출, 디렉토리 제외됨
                //  int deletedFileCount =0;
                if (del_file_get.Checked)
                {
                    foreach (FileSharp file in deletedList)
                    {
                        if (1 != file.GetNType())
                        {
                            if (NTS_Common_Property.fiilter_chk)
                            {
                                #region  필터적용

                                extension = file.GetExtension();

                                if ("" != extension && NTS_Common_Property.NTS_File_Fillter.ContainsKey(extension.ToUpper()))
                                {
                                    if (NTS_Common_Property.search_advanced_option)
                                    {
                                        //기간적용
                                        if (NTS_Common_Property.chk_date(file.get_creationTime(), file.get_accessTime(), file.get_modificationTime()))
                                        {
                                            deletedFiles.Add(file);
                                            deletedAllCount_real_file_count++;
                                        }
                                    }
                                    else
                                    {
                                        //기간 미 적용
                                        deletedFiles.Add(file);
                                        deletedAllCount_real_file_count++;
                                    }


                                }
                                else
                                {

                                }

                                #endregion 필터적용

                            }
                            else
                            {
                                #region 전체목록 확보

                                deletedFiles.Add(file);
                                deletedAllCount_real_file_count++;

                                #endregion
                            }
                        }
                    }


                    //  deletedFileCount = deletedFiles.Count;
                }

                #region 20211118 김준수 수정 - > 12.15 .위쪽에서 위치 변경 하고 전체리스트에서 화보된 리스트만 계산 하게 변경

                file_total_size = fsBuilder.CalculateSize(normalFiles);


                //2021.12.14
                if (del_file_get.Checked)
                {
                    file_total_size += file_total_size + fsBuilder.CalculateSize(deletedFiles);
                }

                Console.WriteLine("전체사이즈:" + NTS_Common_Property.getSize(file_total_size));

                DriveInfo df = new DriveInfo(NTS_Common_Property.copy_position.Substring(0, 1));

                if (df.AvailableFreeSpace <= file_total_size)
                {
                    NTS_Common_Property.free_space_size = file_total_size - df.AvailableFreeSpace;
                    Drv_Process_end("Free_space_not_enough");
                    return;
                };



                #endregion

                _stinfo.set_1("고급모드(정상파일): " + string.Format("{0:#,###}", normalAllCount_real_file_count) +
                              " 고급모드(삭제파일): " + string.Format("{0:#,###}", deletedAllCount_real_file_count) +
                              " / " + NTS_Common_Property.getSize(file_total_size));



                //정상파일, 지워진 파일에 대해 Export 기능 아래에서 수행하면 됨

                long _filecount = 0;
                // long _tot_file_count = 0; // DB index 용 
                Console.WriteLine("----------------------파일 복구 시작-----------------");

                extension = "";
                foreach (FileSharp file_ in normalList)
                {

                    if (NTS_Common_Property.fiilter_chk)
                    {
                        #region  필터적용

                        extension = file_.GetExtension();


                        if ("" != extension && NTS_Common_Property.NTS_File_Fillter.ContainsKey(extension.ToUpper()) && file_.get_nType() == 2)
                        {

                            if (NTS_Common_Property.search_advanced_option)
                            {
                                //기간적용
                                if (NTS_Common_Property.chk_date(file_.get_creationTime(), file_.get_accessTime(), file_.get_modificationTime()))
                                {
                                    #region 파일복사

                                    if (1 != file_.GetNType())
                                    {
                                        FileAccessSharp fileAccess = null;
                                        try
                                        {
                                            fileAccess = new FileAccessSharp(file_);
                                            fileAccess.Open(FileAccessMode.physicalMode);


                                            _filecount++;
                                            this.Invoke(new Action(delegate ()
                                            {
                                                states_Message.Text = "정상파일:" + _filecount + "/" + normalAllCount_real_file_count;


                                            }));


                                            fsBuilder.insert_data(file_, fileAccess, 1); //1 정상파일  2 삭제파일







                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("CFT:에러" + ex.ToString());
                                            continue;
                                        }
                                    }

                                    #endregion
                                }

                            }
                            else
                            {
                                //기간 미적용
                                #region 파일복사

                                if (1 != file_.GetNType())
                                {
                                    FileAccessSharp fileAccess = null;
                                    try
                                    {
                                        //   Console.WriteLine("FileAccessSharp 파일이름:" + file_.get_name() + ":" + NTS_Common_Property.getSize(file_.get_i6Filesize()));
                                        fileAccess = new FileAccessSharp(file_);


                                        fileAccess.Open(FileAccessMode.physicalMode);

                                        _filecount++;
                                        this.Invoke(new Action(delegate ()
                                        {
                                            states_Message.Text = "정상파일:" + _filecount + "/" + normalAllCount_real_file_count;


                                        }));



                                        fsBuilder.insert_data(file_, fileAccess, 1);



                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("CFT:에러" + ex.ToString());
                                        continue;
                                    }
                                }

                                #endregion

                            }
                        }

                        #endregion 필터적용
                    }
                    else
                    {
                        #region 전체파일복사 파일복사

                        if (1 != file_.GetNType())
                        {
                            FileAccessSharp fileAccess = null;
                            try
                            {
                                fileAccess = new FileAccessSharp(file_);
                                fileAccess.Open(FileAccessMode.physicalMode);


                                _filecount++;
                                this.Invoke(new Action(delegate ()
                                {
                                    states_Message.Text = "정상파일:" + _filecount + "/" + normalAllCount_real_file_count;


                                }));


                                fsBuilder.insert_data(file_, fileAccess, 1); //1 정상파일  2 삭제파일







                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("CFT:에러" + ex.ToString());
                                continue;
                            }
                        }

                        #endregion
                    }






                } //for end


                Console.WriteLine("---------------------- 삭제 파일 복구 시작---------------------");


                _filecount = 0;

                if (del_file_get.Checked)
                {

                    foreach (FileSharp file_ in deletedList)
                    {

                        extension = file_.GetExtension();

                        if (NTS_Common_Property.fiilter_chk)
                        {
                            #region 필터 적용

                            if ("" != extension && NTS_Common_Property.NTS_File_Fillter.ContainsKey(extension.ToUpper()) && file_.get_nType() == 2)
                            {

                                if (NTS_Common_Property.search_advanced_option)
                                {
                                    //기간적용
                                    if (NTS_Common_Property.chk_date(file_.get_creationTime(), file_.get_accessTime(), file_.get_modificationTime()))
                                    {
                                        #region 파일복사

                                        if (1 != file_.GetNType())
                                        {
                                            FileAccessSharp fileAccess = null;
                                            try
                                            {
                                                fileAccess = new FileAccessSharp(file_);
                                                fileAccess.Open(FileAccessMode.physicalMode);

                                                _filecount++;
                                                this.Invoke(new Action(delegate ()
                                                {
                                                    states_Message.Text = "삭제파일:" + _filecount + "/" + deletedAllCount_real_file_count;
                                                }));

                                                fsBuilder.insert_data(file_, fileAccess, 2);


                                                //_fc.wirte_advance_del_complete(_filecount + " , " + file_.get_name().Replace("'", "") + " , " +
                                                //    file_.get_extension() + " , " + file_.get_deleted() + " , " +
                                                //    file_.get_i6Filesize() + " , " + file_.get_FilePath() + " , " +
                                                //    file_.get_creationTime() + " , " + file_.get_modificationTime() + " , " +
                                                //    file_.get_accessTime() + " , " + file_.GetMD5Hash_string(false));



                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("CFT:에러" + ex.ToString());
                                                continue;
                                            }
                                        }

                                        #endregion
                                    }


                                }
                                else
                                {
                                    //기간 미적용
                                    #region 파일복사

                                    if (1 != file_.GetNType())
                                    {
                                        FileAccessSharp fileAccess = null;
                                        try
                                        {
                                            fileAccess = new FileAccessSharp(file_);
                                            fileAccess.Open(FileAccessMode.physicalMode);


                                            _filecount++;


                                            this.Invoke(new Action(delegate ()
                                            {
                                                states_Message.Text = "삭제파일:" + _filecount + "/" + deletedAllCount_real_file_count;
                                            }));

                                            fsBuilder.insert_data(file_, fileAccess, 2);


                                            //_fc.wirte_advance_del_complete(_filecount + " , " + file_.get_name().Replace("'", "") + " , " +
                                            //         file_.get_extension() + " , " + file_.get_deleted() + " , " +
                                            //         file_.get_i6Filesize() + " , " + file_.get_FilePath() + " , " +
                                            //         file_.get_creationTime() + " , " + file_.get_modificationTime() + " , " +
                                            //         file_.get_accessTime() + " , " + file_.GetMD5Hash_string(false));


                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("CFT:에러" + ex.ToString());
                                            continue;
                                        }
                                    }

                                    #endregion

                                }
                            }



                            #endregion 필터 적용

                        }
                        else
                        {
                            #region  전체파일 복사

                            if (1 != file_.GetNType())
                            {
                                FileAccessSharp fileAccess = null;
                                try
                                {
                                    fileAccess = new FileAccessSharp(file_);
                                    fileAccess.Open(FileAccessMode.physicalMode);

                                    _filecount++;
                                    this.Invoke(new Action(delegate ()
                                    {
                                        states_Message.Text = "삭제파일:" + _filecount + "/" + deletedAllCount_real_file_count;
                                    }));

                                    fsBuilder.insert_data(file_, fileAccess, 2);


                                    //_fc.wirte_advance_del_complete(_filecount + " , " + file_.get_name().Replace("'", "") + " , " +
                                    //    file_.get_extension() + " , " + file_.get_deleted() + " , " +
                                    //    file_.get_i6Filesize() + " , " + file_.get_FilePath() + " , " +
                                    //    file_.get_creationTime() + " , " + file_.get_modificationTime() + " , " +
                                    //    file_.get_accessTime() + " , " + file_.GetMD5Hash_string(false));



                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("CFT:에러" + ex.ToString());
                                    continue;
                                }
                            }

                            #endregion
                        }
                    }
                }






                #endregion



                fsBuilder.CFT_Tree_Analysis_Process_Persent_send -= new CFT_Module.FileSystemBuilder.CFT_Tree_Analysis_Process_Persent_EventHandler(progressBarControl1_Setting);
                fsBuilder.Drv_Process -= new CFT_Module.FileSystemBuilder.CFT_Drv_Process_EventHandler(states_Message_setp_Setting);
                fsBuilder.File_Copy_Process -= new CFT_Module.FileSystemBuilder.CFT_File_Copy_EventHandler(progressBarControl1_Setting);

                this.Drv_Process_end -= new CFT_Drv_Process_end_EventHandler(cft_end);

                // Application.DoEvents();

                Files_Get_Start_normal(); //네트워크 드라이브 자동 처리 떄문에 일반모드 실행


                //Drv_Process_end("cft_end"); // 국보연 자료 확보 종료 이벤트를 통해 다음 단계(네트워크 폴더) 진행


            });

            th.IsBackground = true;    // 메인 종료시 같이 종료

            th.Start();



            #endregion





            return 0;
        }




        //진행 단계 화면 표시 텍스트
        public delegate void states_Message_setp(string _p);
        public void states_Message_setp_Setting(string _p)
        {

            if (states_Message.InvokeRequired)
            {

                states_Message_setp dr = new states_Message_setp(states_Message_setp_Setting);
                this.Invoke(dr, new object[] { _p });
            }
            else
            {

                if (_p.Equals("imageing_end"))
                {
                    #region 20211118 김준수 수정
                    CFT_Module.FileSystemBuilder fsBuilder = CFT_Module.FileSystemBuilder.GetInstance();
                    #endregion

                    fsBuilder.CFT_imaging_Process_Persent_send -= new CFT_Module.FileSystemBuilder.CFT_imaging_Process_Persent_EventHandler(progressBarControl1_Setting);
                    // MessageBox.Show("이미징 작업 완료");
                    show_alert("진행정보", "이미징이 완료 되었습니다.");
                    start_button_init_on();
                    set_contests(new fs_32.Menu.end_info()); // 시작 정보 화문에 출력

                    if (_fc != null) _fc.close_all();
                }
                else
                {
                    states_Message.Text = _p;
                }
            }

        }


        //진행 단계 화면 표시
        public delegate void progressBarControl1_set(int _p);
        public void progressBarControl1_Setting(int _p)
        {

            if (progressBarControl1.InvokeRequired)
            {

                progressBarControl1_set dr = new progressBarControl1_set(progressBarControl1_Setting);
                this.Invoke(dr, new object[] { _p });
            }
            else
            {
                progressBarControl1.Properties.Maximum = 100;
                progressBarControl1.Position = _p;

                //   Console.WriteLine("값:" + _p);
                if (NTS_Common_Property.logical_imageing)
                {
                    if (progressBarControl1.Position == 100)
                    {
                        #region 20211118 김준수 수정
                        CFT_Module.FileSystemBuilder fsBuilder = CFT_Module.FileSystemBuilder.GetInstance();
                        #endregion

                        fsBuilder.CFT_imaging_Process_Persent_send -= new CFT_Module.FileSystemBuilder.CFT_imaging_Process_Persent_EventHandler(progressBarControl1_Setting);


                        // MessageBox.Show("이미징 작업 완료");
                        if (_fc != null) _fc.close_all();
                        show_alert("진행정보", "이미징이 완료 되었습니다.");
                        start_button_init_on();

                    }

                }
            }

        }


        //국보연 모듈 실행 종료 처라
        public delegate void cft_end_setp(string _p);
        private void cft_end(string _p)
        {

            if (states_Message.InvokeRequired)
            {

                cft_end_setp dr = new cft_end_setp(cft_end);
                this.Invoke(dr, new object[] { _p });
            }
            else
            {
                #region 20211118 김준수 수정
                CFT_Module.FileSystemBuilder fsBuilder = CFT_Module.FileSystemBuilder.GetInstance();
                #endregion

                if (_p.Equals("cft_end"))
                {
                    // this.Drv_Process_end -= new CFT_Drv_Process_end_EventHandler(cft_end);
                    // 일반모드로 네트워크 폴더 진행
                    Working_Step = "네트워크폴더검사";

                    fsBuilder.CFT_Tree_Analysis_Process_Persent_send -= new CFT_Module.FileSystemBuilder.CFT_Tree_Analysis_Process_Persent_EventHandler(progressBarControl1_Setting);
                    fsBuilder.Drv_Process -= new CFT_Module.FileSystemBuilder.CFT_Drv_Process_EventHandler(states_Message_setp_Setting);
                    fsBuilder.File_Copy_Process -= new CFT_Module.FileSystemBuilder.CFT_File_Copy_EventHandler(progressBarControl1_Setting);

                    this.Drv_Process_end -= new CFT_Drv_Process_end_EventHandler(cft_end);

                    //  Application.DoEvents();

                    Files_Get_Start_normal();

                    return;
                };

                if (_p.Equals("Free_space_not_enough"))
                {
                    fsBuilder.CFT_Tree_Analysis_Process_Persent_send -= new CFT_Module.FileSystemBuilder.CFT_Tree_Analysis_Process_Persent_EventHandler(progressBarControl1_Setting);
                    fsBuilder.Drv_Process -= new CFT_Module.FileSystemBuilder.CFT_Drv_Process_EventHandler(states_Message_setp_Setting);
                    fsBuilder.File_Copy_Process -= new CFT_Module.FileSystemBuilder.CFT_File_Copy_EventHandler(progressBarControl1_Setting);

                    this.Drv_Process_end -= new CFT_Drv_Process_end_EventHandler(cft_end);

                    show_alert("시스템 정보", "파일 저장공간이 " + NTS_Common_Property.getSize(NTS_Common_Property.free_space_size) + " 부족 합니다.");

                    //  DB_Controler.Access_Commander.Close_conn();

                    _fc.close_all();

                    m_click("필터설정");
                    set_normal_alalysis(""); // 진행상태 조기화
                    start_button_init_on();

                    return;

                }

            }

        }


        #endregion


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region 일반모드 진행

        string _md5 = "";
        long tot = 0;
        long _normal_f_total_size = 0;
        public List<string> sdrive_list = new List<string>();  // 일반모드 검색드라이브 동적배열 지정 

        string f_name = "";
        string f_ext = "";
        long f_total = 0;



        Stack<string> dirs = new Stack<string>();




        private void Files_Get_Start_normal()
        {


            search_drive_list();

            if (sdrive_list.Count == 0)
            {
                // MessageBox.Show("드라이브를 선택해주세요.");
                show_alert("사용자 오류", "하드디스크를 선택해주세요.");
                start_button_init_on();

                return;
            }


            Thread th = new Thread(() =>
            {

                #region 파일리스트 생성 구간


                SplashThread splash_main = new SplashThread();
                splash_main.Open();
                splash_main.UpdateProgress("자료 검색 중.....", 0);



                // 연결을 유지한 상태에서 디비 입력을 한다.........






                _normal_f_total_size = 0;

                // 선택한 드라이브의 root 파일이 체크가 안되서 11.10 추가
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                string[] files = null;
                string drv = sdrive_list[0];
                try
                {
                    files = Directory.GetFiles(drv, "*.*");

                    foreach (string fn in files)
                    {


                        f_name = Path.GetFileName(fn).Replace(",", "").Replace("'", "");
                        ;
                        f_ext = Path.GetExtension(fn); // 여기서는 국보연 모듈에서는 확장자가 dll 여기서는 .dll로 나와서 결과가 없다

                        if (NTS_Common_Property.fiilter_chk == true)
                        {

                            #region 파일크기가 0 이상이고 필터적용된 파일 리스트 구하기


                            if (f_ext.Length == 0 || f_ext.Equals(""))
                            {
                                //  DB_Controler.Access_Commander.insert_update_Normal_error("확장자가 없는 파일:" + f_name + ":" + currentDir);
                                _fc.wirte_normal_complete_fase("확장자가 없는 파일 , " + f_name.Replace(",", " ") + " : " + drv);
                            }
                            else
                            {

                                f_ext = f_ext.Substring(1, f_ext.Length - 1); // 여기서는 국보연 모듈에서는 확장자가 dll 여기서는 .dll로 나와서 결과가 없다

                                if (NTS_Common_Property.NTS_File_Fillter.ContainsKey(f_ext.ToUpper()))
                                {

                                    FileInfo _ims = new FileInfo(fn); //자장공간 확인 기능 추가로 인해 어쩔수 없이 추가 ... 속도 저하 발생....

                                    if (NTS_Common_Property.search_advanced_option)
                                    {
                                        //기간적용
                                        if (NTS_Common_Property.chk_date(_ims.CreationTime, _ims.LastAccessTime, _ims.LastWriteTime))
                                        {
                                            _normal_f_total_size += _ims.Length;

                                            // 생성일자 적용은 파일 카피할떄 적용
                                            // 파일리스트 작성                                                     
                                            _fc.wirte_normal_complete(f_total + " , " + f_name.Replace(",", " ") + " , " + f_ext + " , " + fn);
                                            f_total++;

                                        }
                                    }
                                    else
                                    {
                                        //기간 미적용
                                        _normal_f_total_size += _ims.Length;
                                        _fc.wirte_normal_complete(f_total + " , " + f_name.Replace(",", " ") + " , " + f_ext + " , " + fn);
                                        f_total++;



                                    }



                                    //일반모드 파일 시스템 분석 단계 화면 갱신 -> 전체겟수를 알수가 없어서 확보된 카운트만 숫자로 표기
                                    set_normal_alalysis("파일검색:" + f_total);

                                }
                            }


                            // Path.
                            //db입력시 파일접근 시간이나 생성시간을  구하기 위해선 New FileInfo 를 써야 하는데 
                            // 목록을 만드는 단계에선 확장자 만을 기준으로 목록을 작성 하도록 한다
                            // 메모리 문제가 발생 할수 있어서
                            // 접근 시간은 파일을 복사 할떄 확인해서 해당 하는것만 확보 하고 해당 리스트 별도 작성

                            #endregion 파일크기가 0 이상이고 필터적용된 파일 리스트 구하기

                        }
                        else
                        {
                            #region 전체파일 확보


                            if (f_ext.Length == 0)
                            {
                                _fc.wirte_normal_complete_fase("파일사이즈 0 , " + f_name.Replace(",", " ") + " : " + drv);
                            }
                            else
                            {

                                f_ext = f_ext.Substring(1, f_ext.Length - 1); // 여기서는 국보연 모듈에서는 확장자가 dll 여기서는 .dll로 나와서 결과가 없다


                                FileInfo _ims = new FileInfo(fn); //자장공간 확인 기능 추가로 인해 어쩔수 없이 추가 ... 속도 저하 발생....

                                if (NTS_Common_Property.search_advanced_option)
                                {
                                    //기간적용
                                    if (NTS_Common_Property.chk_date(_ims.CreationTime, _ims.LastAccessTime, _ims.LastWriteTime))
                                    {
                                        _normal_f_total_size += _ims.Length;

                                        // 생성일자 적용은 파일 카피할떄 적용
                                        // 파일리스트 작성                                                     
                                        _fc.wirte_normal_complete(f_total + " , " + f_name.Replace(",", " ") + " , " + f_ext + " , " + fn);
                                        f_total++;

                                    }
                                }
                                else
                                {
                                    //기간 미적용
                                    _normal_f_total_size += _ims.Length;
                                    _fc.wirte_normal_complete(f_total + " , " + f_name.Replace(",", " ") + " , " + f_ext + " , " + fn);
                                    f_total++;

                                }


                                set_normal_alalysis("파일검색:" + f_total);


                            }



                            #endregion 전체파일 확보

                        }



                    }

                }
                catch (Exception ex)
                {
                    //   Console.WriteLine("에러4" + ex.Message);rib
                    _fc.wirte_normal_complete_fase("확장자가 없는 파일 , " + f_name.Replace(",", " ") + " : " + drv);
                    //  Console.WriteLine("파일명:" + f_name);
                }










                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




                foreach (string s in sdrive_list)
                {
                    #region 선택한 드라이브의 전체 리스트 확보
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(s);
                        FileSystemInfo[] m_dirs = di.GetDirectories("*");

                        //20210810 해당 드라이브 root 에 있는 파일은 검색이 안되는 오류가 있다

                        foreach (FileSystemInfo dn in m_dirs)
                        {
                            if (dn.Exists)
                            {
                                try
                                {
                                    TraverseTree_total(dn.FullName.ToString()); // 드라이브 별로 파일 확인

                                }
                                catch (Exception ex)
                                {

                                    Console.WriteLine("에러1" + ex.Message);
                                    _fc.wirte_normal_complete_fase("파일 존재 하지 않습니다. , " + dn.FullName.ToString().Replace(",", "").Replace("'", ""));
                                    continue;
                                }

                            }
                            else
                            {
                                // 자료 확보중 사용자 파일이 삭제된 경우 처리
                                // DB_Controler.Access_Commander.insert_update_Normal_error("경로가 존재 하지 않습니다.:" + dn.FullName.ToString());
                                _fc.wirte_normal_complete_fase("경로가 존재 하지 않습니다. , " + dn.FullName.ToString().Replace(",", "").Replace("'", ""));
                            }

                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("에러2" + ex.Message);
                        continue;

                    }
                    #endregion


                }




                splash_main.Close();
                splash_main.Join();


                DriveInfo df = new DriveInfo(NTS_Common_Property.copy_position.Substring(0, 1));
                if (df.AvailableFreeSpace <= _normal_f_total_size)
                {
                    NTS_Common_Property.free_space_size = _normal_f_total_size - df.AvailableFreeSpace;


                    this.Invoke(new Action(delegate ()
                    {
                        show_alert("시스템 정보", "파일 저장공간이 " + NTS_Common_Property.getSize(NTS_Common_Property.free_space_size) + " 부족 합니다.");
                        m_click("필터설정");
                        set_normal_alalysis(""); // 진행상태 조기화
                        start_button_init_on();
                        //  DB_Controler.Access_Commander.Close_conn();
                        _fc.close_all();

                    }));





                    return;
                };

                //    string.Format("{0:#,###}",normalAllCount_real_file_count + deletedFileCount) 
                _stinfo.set_2("일반모드 : " + string.Format("{0:#,###}", f_total) + " / " + NTS_Common_Property.getSize(_normal_f_total_size));

                set_normal_alalysis("normail_mode_net_drv_list_complete"); // 파일 분석 완료 신호 전송


                #endregion

            });

            th.IsBackground = true;    // 메인 종료시 같이 종료
            th.Start();

        }


        BackgroundWorker _bw = null;
        private void Save_file_normal_mode_local()
        {

            DataTable _dt = null;
            if (NTS_Common_Property.continue_copy_chk == false)
            {
                _dt = _fc.read_normal_complete();
            }
            else
            {
                _dt = _fc.chk_normal_copy_not_Process();
            }




            _bw = new BackgroundWorker();
            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(work_Dowork);
            _bw.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerComlete);
            _bw.RunWorkerAsync(argument: _dt);

            progressBarControl1.Properties.Maximum = _dt.Rows.Count;

            if (_dt == null || _dt.Rows.Count == 0)
            {
                set_normal_alalysis("확보파일완료");
                return;
            }

        }



        private void work_Dowork(object sender, DoWorkEventArgs e)
        {
            DataTable file = (DataTable)e.Argument;


            if (file == null)
            {
                // 국보연 완료후 null 이면 에러가 남....
                set_normal_alalysis("확보파일완료");
                return;
            }

            int _foldcounter = 0; // 경로 길이가 240자를 넘어가는 파일카운트해서 다른 폴더애 생성
            tot = 0;
            string destFile = "";
            string destfolder = "";

            long fileraad_count = 0; // 
            int screencount = 0;

            bool long_path_chk = false; // long path 는 하나의 long path 에 저장하고 파일이름에 중복번호흫 등록하기 위해서

            foreach (DataRow row in file.Rows)
            {
                tot++;

                destfolder = "";
                destFile = "";
                fileraad_count = 0;

                destfolder = get_save_path().Split('^')[0] + @"\" + get_save_path().Split('^')[1] + @"\" + row[3].ToString().Replace(row[1].ToString(), "").Replace(":", "");
                destFile = destfolder + row[1].ToString();

                long_path_chk = false;



                //원본경로가 길어서 문제가 생기는 경우 파일 복사가 안됨 그래서 Pass
                // 12.13 원본과 복사 경로를 합친 결과가 길어서 변경
                if (NTS_Common_Property.copy_position.Length + row[3].ToString().Length > 200)
                {
                    _foldcounter++;
                    destfolder = NTS_Common_Property.copy_position + "\\" + row[3].ToString().Substring(0, 1) + @"\long_Path";
                    // destfolder = get_save_path().Split('^')[0] + @"\" + get_save_path().Split('^')[1] + "\\" + row[3].ToString().Substring(0, 1) + @"\long Path " + _foldcounter + @"\";
                    destFile = destfolder + @"\" + row[1].ToString().Split('.')[0] + "_" + _foldcounter + "." + row[1].ToString().Split('.')[1];

                    //DB_Controler.Access_Commander.insert_update_copy_Normal_error(
                    //       row[0].ToString(),
                    //       "long_path:" + row[1].ToString().Split('.')[0] + "_" + _foldcounter + "." + row[1].ToString().Split('.')[1],

                    //     row[3].ToString());


                    _fc.wirte_normal_long_path(
                          row[0].ToString() + " , " +
                           "long_path , " + row[1].ToString().Split('.')[0].Replace(",", "") + "_" + _foldcounter + "." + row[1].ToString().Split('.')[1] + " , " +
                           row[3].ToString()
                         );




                    long_path_chk = true;
                    //continue;

                }


                FileInfo info = new FileInfo(row[3].ToString()); //DB에 경로가 c:\test\ 이런 식이라 c:\\test\\ 변경필요
                string _finame = "";

                // 20210821 파일생성일자 적용 여부
                if (NTS_Common_Property.search_advanced_option) // / 파일생성일자 를 검색시 적용 여부
                {

                    if (NTS_Common_Property.chk_date(info.CreationTime, info.LastAccessTime, info.LastWriteTime))
                    {

                        #region


                        //폴더생성
                        if (!System.IO.Directory.Exists(destfolder))
                        {
                            System.IO.Directory.CreateDirectory(destfolder);
                        }



                        // 파일 복사 로직
                        // 원본파일 읽기 파일이 삭제 되었는지 확인하고  DB저장 로직 추가 필요

                        int bufferSize = 1024 * 10;
                        System.IO.Stream inStream = null;
                        try
                        {
                            inStream = new FileStream(row[3].ToString(), FileMode.Open, FileAccess.Read);
                        }
                        catch (Exception ex)
                        {
                            //DB_Controler.Access_Commander.insert_update_copy_Normal_error(row[0].ToString(), "사용중인파일", row[3].ToString());

                            _fc.wirte_normal_copy_complete_fase("사용중인파일 , " + row[3].ToString().Replace(",", ""));


                            continue;
                        }


                        try
                        {
                            using (FileStream fileStream = new FileStream(destFile, FileMode.Create, FileAccess.Write)) // 파일길이 260자 오류 발생
                            {
                                int bytesRead = -1;
                                byte[] bytes = new byte[bufferSize];

                                while ((bytesRead = inStream.Read(bytes, 0, bufferSize)) > 0)
                                {
                                    fileStream.Write(bytes, 0, bytesRead);
                                    fileStream.Flush();

                                    fileraad_count = fileraad_count + bytesRead;

                                    int pl = Getprecenttage(fileraad_count, inStream.Length, 0);

                                    if (pl != screencount)
                                    {
                                        screencount = pl;
                                        _bw.ReportProgress(bytesRead, pl + ":detail:" + 100);
                                        //   Console.WriteLine("진행값" + pl);
                                    }



                                    //3GB READ 읽은 싸이즈가 int 를 넘어가서 진행바에 전달값에 overflow 발생ㅈㅈㅈ

                                }


                                _finame = fileStream.Name;
                                fileStream.Close();
                            }




                            //Console.WriteLine("------------------------ 일반 모드 파일 생성 시간 수정 ----------------------------");
                            //Console.WriteLine(_finame + ":" + info.CreationTime);
                            System.IO.File.SetCreationTime(_finame, info.CreationTime);
                            System.IO.File.SetLastAccessTime(_finame, info.LastAccessTime);
                            System.IO.File.SetLastWriteTime(_finame, info.LastWriteTime);



                        }
                        catch (FileNotFoundException ee)
                        {
                            // Console.WriteLine(destFile + ":" + destFile.Length);
                            //Console.WriteLine("FileNotFoundException");
                            //  DB_Controler.Access_Commander.insert_update_copy_Normal_error(row[0].ToString(), "파일이없습니다,", row[3].ToString());

                            _fc.wirte_normal_copy_complete_fase("사용중인파일 , " + row[3].ToString().Replace(",", ""));
                        }


                        catch (DirectoryNotFoundException xx)
                        {
                            Console.WriteLine("DirectoryNotFoundException");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        //File.SetCreationTime(file.get_name(), file.get_creationTime());
                        //File.SetLastWriteTime(file.get_name(), file.get_modificationTime());
                        //File.SetLastAccessTime(file.get_name(), file.get_accessTime());
                        _md5 = "";
                        if (NTS_Common_Property.ck_md5)
                        {
                            _md5 = getFilesMD5Hash_one(@row[3].ToString());
                        }

                        // DB_Controler.Access_Commander.insert_update_copy_Normal(row, info.CreationTime.ToString(), info.LastWriteTime.ToString(), info.LastAccessTime.ToString(), _md5);
                        //  writer5.WriteLine("번호,파일명,확장자,파일경로,생성일자,최종수정일자,최종조회일자,Md5");
                        _fc.wirte_normal_copy_complete(row[0].ToString() + " , " + row[1].ToString().Replace(",", "") + " , "
                                                 + row[2].ToString() + " , "
                                                 + row[3].ToString().Replace(",", "") + " , "
                           + info.CreationTime.ToString() + " , " + info.LastWriteTime.ToString() + " , " +
                        info.LastAccessTime.ToString() + " , " + _md5);


                        _bw.ReportProgress(0, "일반모드 파일확보:" + tot + "/" + file.Rows.Count);


                        #endregion

                    }


                }
                else
                {

                    #region 위랑 같은 내용 귀찮고 힘들어서 2번 사용



                    //폴더생성
                    if (!System.IO.Directory.Exists(destfolder))
                    {
                        System.IO.Directory.CreateDirectory(destfolder);
                    }

                    //C:\\:h_h_h_h\\E\\$RECYCLE.BIN\\S-1-5-21-2815696863-3790483780-4136488119-500\\"

                    // 파일 복사 로직
                    // 원본파일 읽기 파일이 삭제 되었는지 확인하고  DB저장 로직 추가 필요

                    int bufferSize = 1024 * 10;
                    System.IO.Stream inStream = null;
                    try
                    {
                        inStream = new FileStream(row[3].ToString(), FileMode.Open, FileAccess.Read);
                    }
                    catch (Exception ex)
                    {
                        //DB_Controler.Access_Commander.insert_update_copy_Normal_error(row[0].ToString(), "사용중인파일", row[3].ToString());
                        _fc.wirte_normal_copy_complete_fase(" 사용중인파일 , " + row[3].ToString().Replace(",", ""));
                        continue;
                    }

                    try
                    {
                        using (FileStream fileStream = new FileStream(destFile, FileMode.Create, FileAccess.Write)) // 파일길이 260자 오류 발생
                        {
                            int bytesRead = -1;
                            byte[] bytes = new byte[bufferSize];

                            while ((bytesRead = inStream.Read(bytes, 0, bufferSize)) > 0)
                            {
                                fileStream.Write(bytes, 0, bytesRead);
                                fileStream.Flush();

                                fileraad_count = fileraad_count + bytesRead;

                                int pl = Getprecenttage(fileraad_count, inStream.Length, 0);

                                if (pl != screencount)
                                {
                                    screencount = pl;
                                    _bw.ReportProgress(bytesRead, pl + ":detail:" + 100);
                                    // Console.WriteLine("진행값" + pl);
                                }

                                //3GB READ 읽은 싸이즈가 int 를 넘어가서 진행바에 전달값에 overflow 발생ㅈㅈㅈ


                            }
                            // 확보한 파일의 생성일자 및 수정일자 등을 원본과 동일하게 setting
                            _finame = fileStream.Name;

                            fileStream.Close();


                            //Console.WriteLine("------------------------ 일반 모드 파일 생성 시간 수정 ----------------------------");
                            //Console.WriteLine(_finame + ":" + info.CreationTime);
                            System.IO.File.SetCreationTime(_finame, info.CreationTime);
                            System.IO.File.SetLastAccessTime(_finame, info.LastAccessTime);
                            System.IO.File.SetLastWriteTime(_finame, info.LastWriteTime);


                        }

                    }
                    catch (FileNotFoundException ee)
                    {
                        // Console.WriteLine(destFile + ":" + destFile.Length);
                        // Console.WriteLine("FileNotFoundException");
                        // DB_Controler.Access_Commander.insert_update_copy_Normal_error(row[0].ToString(), "파일이없습니다,", row[3].ToString());
                        _fc.wirte_normal_copy_complete_fase("파일이없습니다 , " + row[3].ToString().Replace(",", ""));
                    }
                    catch (DirectoryNotFoundException xx)
                    {
                        Console.WriteLine("DirectoryNotFoundException");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //File.SetCreationTime(file.get_name(), file.get_creationTime());
                    //File.SetLastWriteTime(file.get_name(), file.get_modificationTime());
                    //File.SetLastAccessTime(file.get_name(), file.get_accessTime());
                    _md5 = "";
                    if (NTS_Common_Property.ck_md5)
                    {
                        _md5 = getFilesMD5Hash_one(@row[3].ToString());
                    }

                    //DB_Controler.Access_Commander.insert_update_copy_Normal(row, info.CreationTime.ToString(), info.LastWriteTime.ToString(), info.LastAccessTime.ToString(), _md5);

                    _fc.wirte_normal_copy_complete(row[0].ToString() + " , " + row[1].ToString().Replace(",", "") + " , "
                                                   + row[2].ToString() + " , "
                                                   + row[3].ToString().Replace(",", "") + " , "
                             + info.CreationTime.ToString() + " , " + info.LastWriteTime.ToString() + " , " +
                          info.LastAccessTime.ToString() + " , " + _md5);

                    _bw.ReportProgress(0, "일반모드 파일확보:" + tot + "/" + file.Rows.Count);


                    #endregion



                }








            }


            set_normal_alalysis("확보파일완료");


        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  Console.WriteLine(e.ProgressPercentage + ":" + e.UserState);
            try
            {
                string[] _Data = e.UserState.ToString().Split(':');


                if (_Data[1].Equals("detail"))
                {
                    progressBarControl1.Properties.Maximum = int.Parse(_Data[2]);
                    // progressBarControl1.Position = e.ProgressPercentage;
                    progressBarControl1.Position = int.Parse(_Data[0]);
                    //  states_Message.Text = _Data[1];
                }
                else
                {

                    // progressBarControl1.Properties.Maximum = int.Parse(_Data[2]);
                    //  progressBarControl1.Position = e.ProgressPercentage;
                    states_Message.Text = e.UserState.ToString();

                }
            }
            catch (Exception ex)
            {

            }


            //   _bw.ReportProgress(tot, file.Rows.Count + ":total:" + file.Rows.Count);

            //    _bw.ReportProgress(bytesRead, file.Rows.Count + ":detail:" + inStream.Length);
            //  Application.DoEvents();
            // 83,852,827
        }

        private void Worker_RunWorkerComlete(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                //  MessageBox.Show(string.Format("Error : {0} ", e.Error.Message));
                show_alert("오류", string.Format("Error : {0} ", e.Error.Message));
            }
            else
            {
                //DB_Controler.Access_Commander.Close_conn();
                //    _fc.close_all();
                //progressBarControl1.EditValue = 0;

                //show_result();

                //start_button_init_on();


                _bw.DoWork -= new DoWorkEventHandler(work_Dowork);
                _bw.ProgressChanged -= new ProgressChangedEventHandler(Worker_ProgressChanged);
                _bw.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(Worker_RunWorkerComlete);


                _bw.CancelAsync();



                //  show_alert("진행정보", "확보작업이 완료 되었습니다.");


                progressBarControl1.EditValue = 0;
                if (_fc != null) _fc.close_all();

                set_contests(new end_info());
                start_button_init_on();


                show_result();




            }
        }

        private void show_result()
        {

            // 원클릭 점겅 항목  DB저장
            //    save_one_path_chk();



            //               DataTable dt1 = null;
            //               DataTable dt2 = null;

            //               if (System.IO.File.Exists(NTS_Common_Property.copy_position + @"\file_system_info.mdb"))
            //               {


            //                    dt1 = DB_Controler.Access_Commander.QueryExecute(@"Select F_del_YN as mess,count(*) as ct from [File_List_advance] group by F_del_YN    
            //                                                                  union all
            //                                                                 Select 'error'  as mess , count(*) as ct from File_List_advance_error ");




            //                    dt2 = DB_Controler.Access_Commander.QueryExecute(@"Select 'File_list' as mess, count(*) as ct from File_List  
            //                                                                                          union all 
            //                                                                                          Select 'File_list_error'  as mess, count(*) as ct from File_List_error
            //                                                                                          union all
            //                                                                                          Select 'File_list_Copy_Comlete'  as mess, count(*) as ct from File_list_Copy_Comlete  
            //                                                                                          union all 
            //                                                                                          Select 'File_list_Copy_Comlete_error'  as mess, count(*) as ct from File_list_Copy_Comlete_error  ");



            //                    // My_Util.display_data(dt2);
            //               }
            //               else
            //               {
            //                    Console.WriteLine("FileList_normal.mdb -> false");
            //               }


            // DB_Controler.Access_Commander.Close_conn();
            _fc.close_all();

            set_contests(new fs_32.Menu.end_info());


            Result_Form dd = new Result_Form();

            dd.StartPosition = FormStartPosition.Manual;
            dd.Location = new Point(this.Location.X + ((this.Width / 2) - (dd.Width / 2)), this.Location.Y + ((this.Height / 2) - (dd.Height / 2)));


            dd.Show();

        }

        private void save_one_path_chk()
        {

            //Thread th = new Thread(() =>
            //{



            for (int i = 0; i < NTS_Common_Property.DT.Rows.Count; i++)
            {
                DB_Controler.Access_Commander.QueryExecute_insert(@"insert into com_info (c_item , c_result  ) 
                                                                 values ('" + NTS_Common_Property.DT.Rows[i]["점검항목"] + "','" + NTS_Common_Property.DT.Rows[i]["점검결과"] + "') ");
            };

            foreach (DataRow row in NTS_Common_Property.DT2.Rows)
            {
                DB_Controler.Access_Commander.QueryExecute_insert(@"insert into usb_use_list (C_no , c_device_name ,
                                                                                                   c_device_serial ,
                                                                                                   c_first_access_time ,
                                                                                                   c_colse_time  )  

                                                      values (" + row["연번"] + ",'" + row["장치명"] + "','" +
                                                                           row["장치명"] + "','" +
                                                                           row["시리얼번호"] + "','" +
                                                                           row["부팅이후최초접근시간"] + "','" +
                                                                           row["마지막연결/해제시간"] + "')");
            }


            for (int ix = 0; ix < NTS_Common_Property.DT3.Rows.Count; ix++)
            {
                DB_Controler.Access_Commander.QueryExecute_insert(@"insert into internet_use_list (C_no , c_url ,
                                                                                                   c_file_name ,
                                                                                                   c_access_date ) 

                                                      values (" + int.Parse(NTS_Common_Property.DT3.Rows[ix][0].ToString()) + ",'" + NTS_Common_Property.DT3.Rows[ix][1].ToString() + "','" +
                                                                                        NTS_Common_Property.DT3.Rows[ix][2].ToString() + "','" +
                                                                                        NTS_Common_Property.DT3.Rows[ix][3].ToString() + "')");
            };


            foreach (DataRow row in NTS_Common_Property.DT4.Rows)
            {
                DB_Controler.Access_Commander.QueryExecute_insert(@"insert into install_Program_list (C_no , c_program_name ,
                                                                                                   c_made_name ,
                                                                                                   c_install_date ,
                                                                                                   c_install_position ,
                                                                                                   c_info  )   
                                                           values (" + row["연번"] + ",'" + row["프로그램명"] + "','" +
                                                                           row["제조사"] + "','" +
                                                                           row["설치일"] + "','" +
                                                                           row["설치위치"] + "','" +
                                                                           row["정보"] + "')");

            }





            //});

            //th.Start();

        }

        private void Save_file_normal_mode_server()
        {
            int iNum = Sever_Folder_Check();

            if (iNum == 2)
            {


                //    MessageBox.Show("전송할 위치에 동일한 업체 및 사용자명이 존재합니다.\n업체 및 사용자명 변경 후 다시 실행하세요!", NTS_Common_Property.PG_Name);

                show_alert("시스템 오류", "전송할 위치에 동일한 업체 및 사용자명이 존재합니다." + Environment.NewLine + "업체 및 사용자명 변경 후 다시 실행하세요!");
                search_copy_flag = false;

                return;
            }
            else if (iNum == 1)
            {
                //  MessageBox.Show("확보 서버에 접속 할 수 없습니다.\n서버 확인 후 다시 실행하세요!", NTS_Common_Property.PG_Name);
                show_alert("시스템 오류", "확보 서버에 접속 할 수 없습니다." + Environment.NewLine + "서버 확인 후 다시 실행하세요!");
                search_copy_flag = false;

                return;
            }


            //////////////////////////////////////////////////////////////////////////////////////////////////

            Thread th = new Thread(() =>
            {
                File_Sever_UPLoad();
            });

            th.Start();




            this.Cursor = System.Windows.Forms.Cursors.Arrow;

        }


        //일반모드 파일 시스템 분석 단계 화면 갱신 -> 전체겟수를 알수가 없어서 확보된 카운트만 숫자로 표기
        public delegate void set_normal_mode_FileSystem_analysis(string cou);
        public void set_normal_alalysis(string _p)
        {
            if (states_Message.InvokeRequired)
            {
                try
                {
                    set_normal_mode_FileSystem_analysis dr = new set_normal_mode_FileSystem_analysis(set_normal_alalysis);
                    this.Invoke(dr, new object[] { _p });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {


                // 일반모드 파일리스트가 DB 에 저장이 왼료 된경우
                if (_p.Equals("normail_mode_net_drv_list_complete"))
                {
                    // Background Work 로 작동
                    if (NTS_Common_Property.local_copy)
                    {
                        Save_file_normal_mode_local();
                    }
                    else
                    {
                        Save_file_normal_mode_server();
                    }

                }
                else if (_p.Equals("확보파일완료"))
                {
                    //  MessageBox.Show("확보작업이 완료 되었습니다");







                }
                else
                {
                    states_Message.Text = _p;
                    // Application.DoEvents();
                }



            }


        }




        #endregion





        // 드라이브 선택시 체크박스 자동변경 기능
        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            if (NTS_Common_Property.search_mode == 0)
            {

                #region 고급모드 Tree Action

                if (e.Node.ParentNode == null)  // 화면이 변경되기전인데 값이 true 변경 되어서 꺼꾸로 체크
                {
                    //root 선택시

                    if (!e.Node.Checked)
                    {
                        e.Node.CheckState = CheckState.Unchecked;
                    }
                    else
                    {
                        e.Node.CheckState = CheckState.Checked;
                        // 부모를 선택 하면 자식은 모두 check 해제
                        for (int i = 0; i < e.Node.Nodes.Count; i++)
                        {
                            e.Node.Nodes[i].CheckState = CheckState.Unchecked;
                        }
                    }


                }
                else
                {
                    // child 선택
                    if (!e.Node.Checked) // 화면이 변경되기전에 값이 true 변경되는것 같다
                    {
                        e.Node.CheckState = CheckState.Unchecked;
                    }
                    else
                    {
                        e.Node.CheckState = CheckState.Checked;

                        e.Node.ParentNode.CheckState = CheckState.Unchecked;

                    }

                }

                #endregion 고급모드 Tree Action
            }
            else
            {

                #region 일반모드 Tree Action
                if (e.Node.ParentNode == null)  // 화면이 변경되기전인데 값이 true 변경 되어서 꺼꾸로 체크
                {
                    //root 선택시

                    if (!e.Node.Checked)
                    {
                        e.Node.CheckState = CheckState.Unchecked;
                        // 부모를 선택 하면 자식은 모두 check 
                        for (int i = 0; i < e.Node.Nodes.Count; i++)
                        {
                            e.Node.Nodes[i].CheckState = CheckState.Unchecked;
                        }
                    }
                    else
                    {


                        e.Node.CheckState = CheckState.Checked;
                        for (int i = 0; i < e.Node.Nodes.Count; i++)
                        {
                            e.Node.Nodes[i].CheckState = CheckState.Checked;
                        }
                    }


                }
                else
                {
                    // child 선택
                    if (!e.Node.Checked) // 화면이 변경되기전에 값이 true 변경되는것 같다
                    {
                        e.Node.CheckState = CheckState.Unchecked;
                        e.Node.ParentNode.CheckState = CheckState.Unchecked;
                    }
                    else
                    {
                        e.Node.CheckState = CheckState.Checked;

                    }

                }

                #endregion 일반모드 Tree Action
            }
        }

        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) != 0)
                popupMenu1.ShowPopup(Control.MousePosition);
        }





        private void set_save_position_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                copy_path.Text = folderBrowserDialog1.SelectedPath;
                // copy_path.Text = "저장위치 : " + Environment.NewLine + folderBrowserDialog1.SelectedPath;


            };
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            foorPprinting_result dd = new foorPprinting_result();
            dd.foot_printing();
            set_contests(dd);


            //Result_Form _dd = new Result_Form(@"C:\test_image\1_1_1_1\");

            //_dd.StartPosition = FormStartPosition.Manual;
            //_dd.Location = new Point(this.Location.X + ((this.Width / 2) - (_dd.Width / 2)), this.Location.Y + ((this.Height / 2) - (_dd.Height / 2)));


            //_dd.Show();

            //    File_Wrie_Controller dd = new File_Wrie_Controller(@"C:\test_image\f_f_f_f");
            //   fs_32_UTIL.My_Util.display_data(dd.chk_normal_copy_not_Process());
            //foot_printing_start();

            //return;


            //  Application.StartupPath + @"\Footprinting\HTML_Export\
            //   NTS_Common_Property.copy_position = @"C:\test_image\d_d_d_d";
            //try
            //{
            //     directory_copy(Application.StartupPath + @"\Footprinting\HTML_Export\", NTS_Common_Property.copy_position + @"\foot_printing\HTML_Export\", false);
            //     directory_copy(Application.StartupPath + @"\Footprinting\CSV_Export\", NTS_Common_Property.copy_position + @"\foot_printing\CSV_Export\", false);
            //}
            //catch (Exception ex)
            //{
            //}

            #region


            //               string _constring = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\test_image\f_f_f_f\G\a_a_a_a\file_system_info.mdb;Jet OLEDB:Database Password=";


            //               DB_Controler.Access_Commander.get_conn2(_constring);



            //               //Console.WriteLine("cominfo:" + DB_Controler.Access_Commander.QueryExecute_insert("delete from com_info"));
            //               //Console.WriteLine("install_Program_list:" + DB_Controler.Access_Commander.QueryExecute_insert("delete from install_Program_list"));
            //               //Console.WriteLine("internet_use_list:" + DB_Controler.Access_Commander.QueryExecute_insert("delete from internet_use_list"));
            //               //Console.WriteLine("usb_use_list:" + DB_Controler.Access_Commander.QueryExecute_insert("delete from usb_use_list"));

            //               //save_one_path_chk();



            //               DataTable dt1 = null;
            //               DataTable dt2 = null;

            //               //   dt1 = DB_Controler.Access_Commander.QueryExecute("Select * from File_List_advance_Error");
            //               //   dt2 = DB_Controler.Access_Commander.QueryExecute("Select * from File_List_Copy_Complete_error");



            //               dt1 = DB_Controler.Access_Commander.QueryExecute(@"Select F_del_YN as mess,count(*) as ct from [File_List_advance] group by F_del_YN    
            //                                                                                 union all
            //                                                                                Select 'error'  as mess , count(*) as ct from File_List_advance_error ");

            //               fs_32_UTIL.My_Util.display_data(dt1);
            //               Console.WriteLine("------------------------------------------------------------------------------");


            //               dt2 = DB_Controler.Access_Commander.QueryExecute(@"Select 'File_list' as mess, count(*) as ct from File_List  
            //                                                                                                         union all 
            //                                                                                                         Select 'File_list_error'  as mess, count(*) as ct from File_List_error
            //                                                                                                         union all
            //                                                                                                         Select 'File_list_Copy_Comlete'  as mess, count(*) as ct from File_list_Copy_Comlete  
            //                                                                                                         union all 
            //                                                                                                         Select 'File_list_Copy_Comlete_error'  as mess, count(*) as ct from File_list_Copy_Comlete_error  ");

            //               fs_32_UTIL.My_Util.display_data(dt2);
            //               Console.WriteLine("------------------------------------------------------------------------------");





            //              // DB_Controler.Access_Commander.Close_conn();
            //               _fc.close_all();
            //               //B_Controler.Access_Commander.Close_conn();

            //               Result_Form dd = new Result_Form(dt1, dt2);
            //               dd.StartPosition = FormStartPosition.Manual;
            //               dd.Location = new Point(this.Location.X + ((this.Width / 2) - (dd.Width / 2)), this.Location.Y + ((this.Height / 2) - (dd.Height / 2)));

            //               dd.Show();

            #endregion
        }


        private void top_Menu7_Load(object sender, EventArgs e)
        {
            //WebBrowser web1 = new WebBrowser();
            //web1.ScriptErrorsSuppressed = true;

            //web1.Url = new Uri(@"file://" + Application.StartupPath + @"\Footprinting\HTML_Export\FootPrinting.htm");



            //Right_Menu_items.Controls.Clear();
            //Right_Menu_items.Padding = new Padding(10, 10, 10, 10);
            //Right_Menu_items.Controls.Add(web1);
            //web1.Dock = DockStyle.Fill;
        }

        // popup 사용자 폴더 추가
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {

                //if (NTS_Common_Property.search_mode == 0)
                //{
                //   //  MessageBox.Show("일반모드에서는 지원 하지 않습니다");

                //     show_alert("시스템 정보", "일반모드에서는 지원 하지 않습니다.");
                //}
                //else
                //{
                TreeListNode nFoder = treeList1.AppendNode(new object[] { folderBrowserDialog1.SelectedPath }, null);
                nFoder.Checked = true;
                nFoder.StateImageIndex = 1;
                //  }

            }
        }

        private void NTS_FS_Main_Form_Load(object sender, EventArgs e)
        {

            if (!Date_Check())
            {
                //  NTS_Common_Property._pPoint.show_alert("시스템오류", "프로그램 사용제한 날짜가 경과하였습니다.");
                //MessageBox.Show("                  프로그램 사용제한 날짜가 경과하였습니다." + Environment.NewLine + Environment.NewLine +
                //              "   프로그램 다운로드 및 인증방법은(내부망) 포렌식관리시스템" + Environment.NewLine +
                //              "            '포렌식프로그램 다운로드 안내'를 참고하세요.", "안내");
                //// Application.Exit();

                NTS_Common_Property._pPoint.show_alert_big("안내", "   프로그램 사용제한 날짜가 경과하였습니다." + Environment.NewLine + Environment.NewLine +
                                 "  [프로그램 다운로드 및 인증방법]" + Environment.NewLine +
                                 "  내부망->대내포털시스템->포렌식관리시스템-> " + Environment.NewLine + "'포렌식 프로그램 다운로드' 참조");

                Application.Exit();

            }
            else
            {

                chk_key();
            }








            copy_path.Text = NTS_Common_Property.Exe_Path;


            //FileInfo _fi = new FileInfo(Application.StartupPath + @"\Footprinting\HTML_Export\FootPrinting.htm");
            FileInfo _fi = new FileInfo(Application.StartupPath + @"\FootprintingStandalone\Footprinting\HTML_Export\FootPrinting.htm");
            //FileInfo _fi = new FileInfo(@"C:\HTML_Export\FootPrinting.htm");



            if (_fi.Exists)
            {
                _fi.Delete();
            }


            //DirectoryInfo _di = new DirectoryInfo(Application.StartupPath + @"\Footprinting\CSV_Export");
            DirectoryInfo _di = new DirectoryInfo(Application.StartupPath + @"\FootprintingStandalone\Footprinting\CSV_Export");
            //DirectoryInfo _di = new DirectoryInfo(@"C:\Footprinting\CSV_Export");

            if (_di.Exists)
            {

                System.IO.FileInfo[] _files = _di.GetFiles("*.*", SearchOption.AllDirectories);

                foreach (FileInfo file in _files)
                {
                    file.Attributes = FileAttributes.Normal;
                }
                //Directory.Delete(Application.StartupPath + @"\Footprinting\CSV_Export", true);
                Directory.Delete(Application.StartupPath + @"\FootprintingStandalone\Footprinting\CSV_Export", true);
                //Directory.Delete(Application.StartupPath + @"C:\Footprinting\CSV_Export", true);

            }




        }


        private string get_parent()
        {

            try
            {
                System.IO.DirectoryInfo dirinfo = System.IO.Directory.GetParent(Application.StartupPath);
                return dirinfo.FullName;
            }
            catch (Exception ex)
            {
            }

            return "";
        }


        string in_ip = "10.13.200.85"; // 내부망 ping test
        string ex_ip = "10.12.187.56";   // 외부망 ping test

        private bool chk_ping(string ip)
        {
            try
            {
                System.Net.NetworkInformation.Ping pPing = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingOptions pPingOption = new System.Net.NetworkInformation.PingOptions();

                pPingOption.DontFragment = true;

                string strData = "123123123";

                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(strData);

                int timeout = 120;

                PingReply reply = pPing.Send(ip, timeout, buffer, pPingOption);

                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }




            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void chk_key()
        {

            bool x1 = chk_ping(ex_ip);
            bool x2 = chk_ping(in_ip);
            if (x1 || x2)
            {
                // 내부망이거나 외부망이면 라이센스 체크 PASS

                return;
            }

            try
            {
                //  string FolderPath = System.IO.Directory.GetCurrentDirectory() + "\\.." + "\\FantasyLicense.key";  //선택폴더명 저장    
                string FolderPath = get_parent() + "\\NFT_License.key";  //선택폴더명 저장    

                if (!File.Exists(FolderPath))
                {
                    //show_alert("진행정보", "인증 정보 파일이 존재하지 않습니다." );
                    //
                    // 해당기능 숨김 22.01.05 요청으로 안내만 하고 동일 기능 제공
                    //   NTS_Common_Property.License_key_chk = false;
                    //   NTS_Common_Property.search_mode = 1;


                    //   li_text.Text = "- 인증 정보가 없습니다.";

                }
                else
                {

                    StreamReader sr = new StreamReader(FolderPath, System.Text.Encoding.Default, true);
                    string line = sr.ReadToEnd();
                    sr.Close();


                    //-> FANTASYMASTERKEY # 2021.12.31 #192.168.23.150 # d_d

                    IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());

                    string[] _data = Security.DecryptString(line, "P@ssw0rd").Split('#');

                    bool _chk_lic = true;

                    if (_data.Length != 4)
                    {
                        // MessageBox.Show("라이센스 파일에 문제가 있습니다.");
                        _chk_lic = false;
                    }

                    DateTime dd = Convert.ToDateTime(_data[1].Replace(".", "-"));
                    System.DateTime date2 = DateTime.Now;
                    if (dd < date2)
                    {
                        // MessageBox.Show("기간이 만료 되었습니다.");
                        _chk_lic = false;
                    }

                    //if (!_data[2].Equals(IPHost.AddressList[0].ToString()))
                    //{
                    //   //  MessageBox.Show("라이센스 파일에 문제가 있습니다.");
                    //    // this.Dispose();
                    //     _chk_lic = false;
                    //}


                    if (_chk_lic == false)
                    {


                        // 해당기능 숨김 22.01.05 요청으로 안내만 하고 동일 기능 제공
                        show_alert("진행정보", "인증 기간이 종료 되었 습니다.");
                        //NTS_Common_Property.License_key_chk = false;
                        //NTS_Common_Property.search_mode = 1;

                        li_text.Text = "-인증 정보가 없습니다.";

                    }



                    //RegistryKey rkUsb = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PCheck\Key", false);
                    //if (rkUsb == null)
                    //{
                    //     MessageBox.Show("라이센스 파일이 등록 되지 않았습니다.");
                    //     this.Dispose();
                    //}
                    //   Console.WriteLine("레지스터:" + Security.DecryptString(rkUsb.GetValue("LicenseKey").ToString(), "P@ssw0rd"));


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("인증 파일에 문제가 있습니다.");
                return;
            }
        }



        private bool Date_Check()
        {

            try
            {
                System.DateTime date1 = new System.DateTime(Convert.ToInt16(Main_Variable.datelimit.Substring(0, 4)),
                                                            Convert.ToInt16(Main_Variable.datelimit.Substring(5, 2)),
                                                            Convert.ToInt16(Main_Variable.datelimit.Substring(8, 2)), 00, 00, 0);
                System.DateTime date2 = DateTime.Now;

                if (date2 > date1)
                {
                    return false;  // 사용기간이 지난 경우
                }

                string date2_key = Security.EncryptString(date2.ToString(), "yoon");

                WindowsIdentity id = WindowsIdentity.GetCurrent();
                string sid = id.User.Value.ToString();


                string sBasePath = sid + @"\Software\NTS\User";
                RegistryKey rkRoot = Registry.Users.OpenSubKey(sBasePath, true);
                if (rkRoot == null)
                {


                    RegistryKey reg1 = Registry.Users;
                    reg1 = Registry.Users;
                    reg1 = Microsoft.Win32.Registry.Users.CreateSubKey(sid + @"\Software\NTS\User\", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    reg1.SetValue("Date", date2_key, RegistryValueKind.String);
                    reg1.Close();


                }
                else  //값이 있을 경우 처리
                {

                    //1 . 현재 일자가 최종사용일자 보다 작으면 사용자가 날자를 변경한 경우 false

                    string ss = rkRoot.GetValue("Date", true).ToString();
                    ss = Security.DecryptString(ss, "yoon").ToString();

                    string date_check = Security.DecryptString(rkRoot.GetValue("Date", true).ToString(), "yoon").ToString();
                    System.DateTime date3 = Convert.ToDateTime(date_check); //최종사용일자

                    if (date2 < date3)
                    {
                        MessageBox.Show("시스템 날자가 변경 되었습니다.", "안내");
                        return false;  // 사용자가 날자를 조작한 경우
                    }

                    RegistryKey reg1 = Registry.Users;
                    reg1 = Registry.Users;
                    reg1 = Microsoft.Win32.Registry.Users.CreateSubKey(sid + @"\Software\NTS\User\", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    reg1.SetValue("Date", date2_key, RegistryValueKind.String);
                    reg1.Close();



                }




            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
            #region
            //// 제한일자
            //System.DateTime date1 = new System.DateTime(Convert.ToInt16(Main_Variable.datelimit.Substring(0, 4)), Convert.ToInt16(Main_Variable.datelimit.Substring(5, 2)), Convert.ToInt16(Main_Variable.datelimit.Substring(8, 2)), 00, 00, 0);
            ////현재일자
            //System.DateTime date2 = DateTime.Now;



            //if (date1 < date2)
            //{
            //     // MessageBox.Show("사용기간이 종료 되었습니다.");
            //     // Main_Variable.Del_Over = true;
            //     //    MessageBox.Show("프로그램 등록값이 변경되었습니다.", Main_Variable.PG_Name);
            //     return false;
            //}
            //else
            //{

            //     // 사용가능 한 경유 최종 사용일자를 레스트에 등록 사용자가 날자를 변경하면 최종사용 날자와 비교 
            //     // -> 사용자가 최종 사용일자 이전으로 날자를 바꾸면 사용이 불가 하도록 해서 날자 제한을 완벽하게 구현

            //     string date2_key = Security.EncryptString(date2.ToString(), "Hoon");

            //     Console.WriteLine(date2_key.ToString());

            //     //1. 한번이라도 사용기간 초과 메시지 나오면   -> 레지스터에 최종일자를 만료일자로 변경 날자를 이전으로 돌려도 안되도록
            //     //2. 현재 일자가 최종 사용일자 보다 이전이면  -> 레지스터에 최종일자를 만료일자로 변경 날자를 이전으로 돌려도 안되도록

            //     WindowsIdentity id = WindowsIdentity.GetCurrent();
            //     string sid = id.User.Value.ToString();
            //     string sBasePath = sid + @"\Software\PCheck\Lastdate";

            //     RegistryKey rkRoot = Registry.Users.OpenSubKey(sBasePath, true);

            //     if (rkRoot == null)
            //     {
            //          // 최초 사용일경우 현재 일자를 최근 사용일자를 
            //          RegistryKey reg3 = Registry.Users;
            //          reg3 = Registry.Users;
            //          reg3 = Microsoft.Win32.Registry.Users.CreateSubKey(sid + @"\Software\PCheck\Lastdate\", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //          reg3.SetValue("Ver", date2_key);
            //          reg3.Close();


            //     }
            //     else
            //     {                      


            //          string date_check = Security.DecryptString(rkRoot.GetValue("Ver", true).ToString(), "Hoon"); // 레지스터 최종사용일자 읽기

            //          System.DateTime date3 = new System.DateTime(Convert.ToInt16(date_check.Substring(0, 4)), 
            //                                                      Convert.ToInt16(date_check.Substring(5, 2)),
            //                                                      Convert.ToInt16(date_check.Substring(8, 2)), 00, 00, 0); // 최종사용일자



            //          if (date3 > date2)
            //          {
            //               //최종사용일자가 현재 일자 이후이면 날자를 변경한 경우라
            //               return false;
            //          }


            //          RegistryKey reg3 = Registry.Users;
            //          reg3 = Registry.Users;
            //          reg3 = Microsoft.Win32.Registry.Users.CreateSubKey(sid + @"\Software\PCheck\Lastdate\", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //          reg3.SetValue("Ver", date2_key);
            //          reg3.Close();





            //     }
            //     return true;
            #endregion
            #region


            //try
            //{
            //    // 제한일자
            //    System.DateTime date1 = new System.DateTime(Convert.ToInt16(Main_Variable.datelimit.Substring(0, 4)), Convert.ToInt16(Main_Variable.datelimit.Substring(5, 2)), Convert.ToInt16(Main_Variable.datelimit.Substring(8, 2)), 00, 00, 0);
            //    //현재일자
            //    System.DateTime date2 = DateTime.Now;

            //    string date2_key = Security.EncryptString(date2.ToString(), "Hoon");

            //    WindowsIdentity id = WindowsIdentity.GetCurrent();
            //    string sid = id.User.Value.ToString();


            //    string sBasePath = sid + @"\Software\NTS_FS\User";
            //    RegistryKey rkRoot = Registry.Users.OpenSubKey(sBasePath, true);
            //    if (rkRoot == null)
            //    {


            //        string sBasePath_Check = sid + @"\Software\PCheck\Num";
            //        RegistryKey rkRoot_Check = Registry.Users.OpenSubKey(sBasePath_Check, true);

            //        if (rkRoot_Check != null)
            //        {


            //            if (String.Compare(rkRoot_Check.ToString(), Main_Variable.ver) >= 0)
            //            {

            //                return false;
            //            }

            //        }


            //        RegistryKey reg3 = Registry.Users;
            //        reg3 = Registry.Users;
            //        reg3 = Microsoft.Win32.Registry.Users.CreateSubKey(sid + @"\Software\PCheck\Num\", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //        reg3.SetValue("Ver", Main_Variable.ver, RegistryValueKind.String);
            //        reg3.Close();

            //        RegistryKey reg1 = Registry.Users;
            //        reg1 = Registry.Users;
            //        reg1 = Microsoft.Win32.Registry.Users.CreateSubKey(sid + @"\Software\NTS_CFP\User\", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //        reg1.SetValue("Date", date2_key, RegistryValueKind.String);
            //        reg1.Close();


            //    }
            //    else  //값이 있을 경우 처리
            //    {


            //        RegistryKey reg3 = Registry.Users;
            //        reg3 = Registry.Users;
            //        reg3 = Microsoft.Win32.Registry.Users.CreateSubKey(sid + @"\Software\PCheck\Num\", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //        reg3.SetValue("Ver", Main_Variable.ver, RegistryValueKind.String);
            //        reg3.Close();



            //        string date_check = rkRoot.GetValue("Date", true).ToString();

            //        try
            //        {
            //            System.DateTime date3 = Convert.ToDateTime(Security.DecryptString(date_check, "Hoon"));
            //            System.TimeSpan diff2 = date3.Subtract(date2);

            //            System.DateTime date4 = new System.DateTime(Convert.ToInt16(Main_Variable.timelimit.Substring(0, 4)), Convert.ToInt16(Main_Variable.timelimit.Substring(5, 2)), Convert.ToInt16(Main_Variable.timelimit.Substring(8, 2)), 00, 00, 0);
            //            System.TimeSpan diff4 = date4.Subtract(date3);

            //            if (diff4.TotalDays < 0) Main_Variable.Del_Over = true;


            //            if (diff2.TotalDays < 0)
            //            {


            //                RegistryKey reg1 = Registry.Users;
            //                reg1 = Registry.Users;
            //                reg1 = Microsoft.Win32.Registry.Users.CreateSubKey(sid + @"\Software\NTS_CFP\User\", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //                reg1.SetValue("Date", date2_key, RegistryValueKind.String);
            //                reg1.Close();


            //            }
            //            else
            //            {
            //                date2 = date3;
            //            }


            //        }
            //        catch (Exception)
            //        {
            //            Main_Variable.Del_Over = true;
            //            //    MessageBox.Show("프로그램 등록값이 변경되었습니다.", Main_Variable.PG_Name);
            //            return false;
            //        }

            //    }


            //    System.TimeSpan diff1 = date1.Subtract(date2);

            //    if (diff1.TotalDays < 0)
            //    {
            //        Main_Variable.Del_Over = true;
            //        //  MessageBox.Show("프로그램 사용제한 날짜가 경과하였습니다.", Main_Variable.PG_Name);
            //        return false;

            //    }

            //}
            //catch { }

            //  return true;
            #endregion

        }


        private void NTS_FS_Main_Form_Shown(object sender, EventArgs e)
        {



            init_data();
            init_fillter();



            show_alertNY("진행정보", "풋프린팅을 진행하시겠습니까?");
            if (NTS_Common_Property.Alert_Form_YN_plag == false)
            {
                FoorPrinting_State("사전분석이 취소 되었습니다.");

            }
            else
            {
                foot_printing_start();
            }



            #region

            //Result_Foot_Printing _ims = new Result_Foot_Printing();
            //_ims.StartPosition = FormStartPosition.Manual;
            //_ims.Location = new Point(this.Location.X + 500, this.Location.Y + 150);
            //_ims.Show();

            //  Result_Foot_Printing _ims = new Result_Foot_Printing();
            //   _ims.Show();

            //if (NTS_Common_Property._pPoint.process_start)
            //{
            //   
            //     return;
            //}


            ///////////2022.01.06    NTS_Common_Property._pPoint.show_alert("진행 정보", "백신 프로그램" + Environment.NewLine + " 'PC 실시간 검사' 기능을 꺼 주세요.");




            //string ddx = Application.StartupPath.Substring(0, 2);


            ///////////////// 현재 폴더를 제외한 나모지자동 선택
            //for (int i = 0; i < treeList1.Nodes.TreeList.AllNodesCount; i++)
            //{
            //     TreeListNode ff = treeList1.GetNodeByVisibleIndex(i);

            //     Console.WriteLine("값:" + ff.GetValue("드라이브1").ToString());

            //     try
            //     {

            //          if (ff.GetValue("드라이브1").ToString().Substring(0, 2).ToUpper().Equals("PH"))
            //          {
            //               ff.CheckState = CheckState.Checked;
            //          }

            //     }
            //     catch (Exception ex)
            //     {
            //          Console.WriteLine("일반모드는 드라이브2 내용이 없어서");
            //     }


            //}


            //for (int i = 0; i < treeList1.Nodes.TreeList.AllNodesCount; i++)
            //{
            //     TreeListNode ff = treeList1.GetNodeByVisibleIndex(i);

            //     try
            //     {
            //          if (ff.GetValue("드라이브1").ToString().Substring(0, 2).Equals(ddx))
            //          {

            //               //ff.CheckState = CheckState.Checked;
            //               // 부모를 선택 하면 자식은 모두 check 해제
            //               ff.ParentNode.UncheckAll();
            //               for (int j = 0; j < ff.ParentNode.Nodes.Count; j++)
            //               {
            //                    //   e.Node.Nodes[i].CheckState = CheckState.Unchecked;

            //                    ff.Nodes[j].CheckState = CheckState.Unchecked;
            //               }

            //          }
            //     }
            //     catch (Exception ex)
            //     {
            //          Console.WriteLine("일반모드는 드라이브2 내용이 없어서");
            //     }


            //}
















            //if (e.Node.ParentNode == null)  // 화면이 변경되기전인데 값이 true 변경 되어서 꺼꾸로 체크
            //    {
            //         //root 선택시

            //         if (!e.Node.Checked)
            //         {
            //              e.Node.CheckState = CheckState.Unchecked;
            //         }
            //         else
            //         {
            //              e.Node.CheckState = CheckState.Checked;
            //              // 부모를 선택 하면 자식은 모두 check 해제
            //              for (int i = 0; i < e.Node.Nodes.Count; i++)
            //              {
            //                   e.Node.Nodes[i].CheckState = CheckState.Unchecked;
            //              }
            //         }


            //    }
            //    else
            //    {
            //         // child 선택
            //         if (!e.Node.Checked) // 화면이 변경되기전에 값이 true 변경되는것 같다
            //         {
            //              e.Node.CheckState = CheckState.Unchecked;
            //         }
            //         else
            //         {
            //              e.Node.CheckState = CheckState.Checked;

            //              e.Node.ParentNode.CheckState = CheckState.Unchecked;

            //         }

            //    }





            ////////////////////////////////////////////////








            #endregion



            #region
            //XtraMessageBox.AllowCustomLookAndFeel = true;
            //var rest = XtraMessageBox.Show("사전점검을 시작하시겠습니까?", "안내", MessageBoxButtons.YesNo);
            ////Footrinting _ims = new Footrinting();
            ////_ims.Foot_Counter += new Footrinting.Foot_Printing_EventHandler(_foot_count);

            //if (rest == DialogResult.Yes)
            //{

            //     foot_printing_result ddx = new foot_printing_result();
            //     ddx.Show();

            //     //Thread th = new Thread(() =>
            //     //{

            //     //    _ims.Footrinting_start();

            //     //});

            //     //th.Start();
            //}
            //else
            //{
            //     // MessageBox.Show("no");
            //}



            ////  XtraMessageBox.AllowCustomLookAndFeel = true;
            //  var rest = MessageBox.Show("사전점검을 시작하시겠습니까?", "안내", MessageBoxButtons.YesNo);


            //  if (rest == DialogResult.Yes)
            //  {
            //      Thread th = new Thread(() =>
            //      {
            //            Footrinting _ims = new Footrinting();
            //                        _ims.Foot_Counter += new Footrinting.Foot_Printing_EventHandler(_foot_count);

            //          _ims.Footrinting_start();

            //      });

            //      th.Start();
            //      Console.WriteLine("쓰레드 시작");
            //  }
            //  else
            //  {
            //      // MessageBox.Show("no");
            //  }


            #endregion
        }



        ///////////////////////////////////////////  함수 영역 ///////////////////////////////////

        // DB 파일 생성과 Table 생성
        private bool create_db()
        {
            bool ret = true;


            if (!DB_Controler.Access_Commander.Create_DB_File(NTS_Common_Property.copy_position + @"\file_system_info.mdb"))
            {
                return false;
            }
            if (!DB_Controler.Access_Commander.Create_table())
            {
                return false;
            }

            return ret;
        }

        private int Sever_Folder_Check()
        {

            int iNum = 0;


            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(NTS_Common_Property.server_ip, Convert.ToInt32(NTS_Common_Property.server_port));
                NetworkStream networkStream = tcpClient.GetStream();



                try
                {
                    if (networkStream.CanWrite && networkStream.CanRead)
                    {


                        Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("1|" + NTS_Common_Property.copy_position);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);




                        byte[] ReadByte;
                        ReadByte = new byte[tcpClient.ReceiveBufferSize];
                        int BytesRead = networkStream.Read(ReadByte, 0, (int)ReadByte.Length);
                        string Exist = System.Text.Encoding.UTF8.GetString(ReadByte, 0, BytesRead);


                        string[] szFileInfo = Exist.Split('|');
                        // NO TRUE 500

                        if (szFileInfo[0].ToString() == "YES") iNum = 2;

                        if (szFileInfo[1].ToString().ToUpper() == "TRUE") NTS_Common_Property.limit = true;
                        else NTS_Common_Property.limit = false;

                        NTS_Common_Property.M = Convert.ToDecimal(szFileInfo[2].ToString());


                    }
                }
                catch (SocketException)
                {
                    iNum = 1;
                }
                catch (System.IO.IOException)
                {
                    iNum = 1;
                }
                catch (Exception ex)
                {
                    iNum = 1;

                }
                finally
                {


                    networkStream.Flush();
                    networkStream.Close();

                    tcpClient.Close();


                }
            }
            catch (Exception ex)
            {
                iNum = 1;
            }



            return iNum;




        }

        private bool chk_not_copy()
        {

            string _sql = @"select * from file_list a where not exists
                                        ( select x.f_no from 
                                                        (
                                                           select  f_no from File_List_Copy_Comlete
                                                           union all
                                                           select  f_no from File_List_Copy_Comlete_error
                                                        ) x 
                                           where x.f_no = a.f_no
                                        )
                              ";

            string[] _path = NTS_Common_Property._pPoint.get_save_path().Split('^');
            string save_path = _path[0] + @"\" + _path[1];
            string f2 = save_path + @"\FileList_normal.mdb";

            DataTable dt2 = null;

            if (System.IO.File.Exists(f2))
            {

                if (System.IO.File.Exists(f2))
                {
                    Console.WriteLine("FileList_normal.mdb -> ok");


                    dt2 = DB_Controler.Access_Commander.QueryExecute(_sql);


                    if (dt2.Rows.Count > 0)
                    {
                        // DialogResult result = MessageBox.Show("확보되지 않은 파일이 있습니다.\r\n이어서 계속 진행하시겠습니까?", "pg", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        show_alertYN("진행정보", "이어서 계속 진행하시겠습니까?");
                        if (NTS_Common_Property.Alert_Form_YN_plag)
                        {
                            // File_List_Complete  테이블에 없는 항목만 복사
                            NTS_Common_Property.continue_copy_chk = true;
                            Save_file_normal_mode_local();
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                }

            }

            return true;


        }

        private void search_drive_list_net()
        {

            sdrive_list.Clear();


            for (int i = 0; i < treeList1.Nodes.TreeList.AllNodesCount; i++)
            {
                TreeListNode ff = treeList1.GetNodeByVisibleIndex(i);

                if (ff.GetValue("드라이브").ToString().IndexOf(@"\.\") == -1)
                {

                    if (ff.Checked == true)
                    {
                        if (ff.GetValue("드라이브").ToString().IndexOf("Network") > -1)
                        {
                            sdrive_list.Add(ff.GetValue("드라이브").ToString().Substring(0, 3));
                        }
                        else
                        {
                            sdrive_list.Add(ff.GetValue("드라이브").ToString() + @"\");
                        }
                        //Console.WriteLine(ff.GetValue("드라이브").ToString().Substring(0, 3));
                    }
                }

                // Console.WriteLine(treeList1.GetNodeByVisibleIndex(i).Nodes[0].GetValue(0).ToString());
            }


        }

        private void search_drive_list()
        {

            sdrive_list.Clear();


            for (int i = 0; i < treeList1.Nodes.TreeList.AllNodesCount; i++)
            {
                TreeListNode ff = treeList1.GetNodeByVisibleIndex(i);

                if (ff.GetValue("드라이브").ToString().IndexOf(@"\.\") == -1)
                {

                    if (ff.Checked == true)
                    {
                        if (ff.GetValue("드라이브").ToString().IndexOf("Fixed") > -1 || ff.GetValue("드라이브").ToString().IndexOf("Network") > -1)
                        {
                            sdrive_list.Add(ff.GetValue("드라이브").ToString().Substring(0, 3));
                        }
                        else
                        {
                            sdrive_list.Add(ff.GetValue("드라이브").ToString() + @"\");
                        }
                        //Console.WriteLine(ff.GetValue("드라이브").ToString().Substring(0, 3));
                    }
                }

                // Console.WriteLine(treeList1.GetNodeByVisibleIndex(i).Nodes[0].GetValue(0).ToString());
            }


        }


        public string get_save_path()
        {

            // "^" 를 하는 이유는 경우에 따라 뒤쪽 부분만 필요한 경우가 있어서
            return copy_path.Text + "^" + txt_company.Text.Trim() + "_" + txt_depart.Text.Trim() + "_" + cb_position.Text.Trim() + "_" + txt_pcuser.Text.Trim();

        }

        private int Getprecenttage(decimal _val, decimal _tot, int decimalplaces)
        {
            return int.Parse(System.Math.Round(_val * 100 / _tot, decimalplaces).ToString());
        }

        public string getFilesMD5Hash_one(string file)
        {
            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);

                md5.ComputeHash(stream);
                stream.Close();
                byte[] hash = md5.Hash;

                StringBuilder sb = new StringBuilder();

                foreach (byte b in hash)
                {
                    sb.Append(string.Format("{0:X2}", b));
                }

                stream = null;

                return sb.ToString();

            }
            catch (Exception)
            {
                return "생성오류";
            }
            finally
            {

            }
        }



        private void TraverseTree_total(string root)
        {

            dirs.Push(root);


            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;

                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch (Exception ex)
                {
                    // ex.Message -> DB에 저장 해당 경로에 대한 접근 오류 발생
                    // DB_Controler.Access_Commander.insert_update_Normal_error("경로에 대한 접근 오류 발생:" + ex.Message);
                    _fc.wirte_normal_complete_fase("경로에 대한 접근 오류 발생 , " + ex.Message.Replace(Environment.NewLine, ""));


                    continue;
                }

                // catch (UnauthorizedAccessException e)   { //  Console.WriteLine(e.Message);  continue;  }
                // catch (System.IO.DirectoryNotFoundException e){//Console.WriteLine(e.Message); continue;  }

                string[] files = null;

                try
                {


                    try
                    {
                        files = Directory.GetFiles(currentDir, "*.*");

                        foreach (string fn in files)
                        {


                            f_name = Path.GetFileName(fn).Replace(",", "").Replace("'", "");
                            ;
                            f_ext = Path.GetExtension(fn); // 여기서는 국보연 모듈에서는 확장자가 dll 여기서는 .dll로 나와서 결과가 없다

                            if (NTS_Common_Property.fiilter_chk == true)
                            {

                                #region 파일크기가 0 이상이고 필터적용된 파일 리스트 구하기


                                if (f_ext.Length == 0 || f_ext.Equals(""))
                                {
                                    //  DB_Controler.Access_Commander.insert_update_Normal_error("확장자가 없는 파일:" + f_name + ":" + currentDir);
                                    _fc.wirte_normal_complete_fase("확장자가 없는 파일 , " + f_name.Replace(",", " ") + " : " + currentDir);
                                }
                                else
                                {

                                    f_ext = f_ext.Substring(1, f_ext.Length - 1); // 여기서는 국보연 모듈에서는 확장자가 dll 여기서는 .dll로 나와서 결과가 없다

                                    if (NTS_Common_Property.NTS_File_Fillter.ContainsKey(f_ext.ToUpper()))
                                    {

                                        FileInfo _ims = new FileInfo(fn); //자장공간 확인 기능 추가로 인해 어쩔수 없이 추가 ... 속도 저하 발생....

                                        if (NTS_Common_Property.search_advanced_option)
                                        {
                                            //기간적용
                                            if (NTS_Common_Property.chk_date(_ims.CreationTime, _ims.LastAccessTime, _ims.LastWriteTime))
                                            {
                                                _normal_f_total_size += _ims.Length;

                                                // 생성일자 적용은 파일 카피할떄 적용
                                                // 파일리스트 작성                                                     
                                                _fc.wirte_normal_complete(f_total + " , " + f_name.Replace(",", " ") + " , " + f_ext + " , " + fn);
                                                f_total++;

                                            }
                                        }
                                        else
                                        {
                                            //기간 미적용
                                            _normal_f_total_size += _ims.Length;
                                            _fc.wirte_normal_complete(f_total + " , " + f_name.Replace(",", " ") + " , " + f_ext + " , " + fn);
                                            f_total++;



                                        }



                                        //일반모드 파일 시스템 분석 단계 화면 갱신 -> 전체겟수를 알수가 없어서 확보된 카운트만 숫자로 표기
                                        set_normal_alalysis("파일검색:" + f_total);

                                    }
                                }


                                // Path.
                                //db입력시 파일접근 시간이나 생성시간을  구하기 위해선 New FileInfo 를 써야 하는데 
                                // 목록을 만드는 단계에선 확장자 만을 기준으로 목록을 작성 하도록 한다
                                // 메모리 문제가 발생 할수 있어서
                                // 접근 시간은 파일을 복사 할떄 확인해서 해당 하는것만 확보 하고 해당 리스트 별도 작성

                                #endregion 파일크기가 0 이상이고 필터적용된 파일 리스트 구하기

                            }
                            else
                            {
                                #region 전체파일 확보


                                if (f_ext.Length == 0)
                                {
                                    _fc.wirte_normal_complete_fase("파일사이즈 0 , " + f_name.Replace(",", " ") + " : " + currentDir);
                                }
                                else
                                {

                                    f_ext = f_ext.Substring(1, f_ext.Length - 1); // 여기서는 국보연 모듈에서는 확장자가 dll 여기서는 .dll로 나와서 결과가 없다


                                    FileInfo _ims = new FileInfo(fn); //자장공간 확인 기능 추가로 인해 어쩔수 없이 추가 ... 속도 저하 발생....

                                    if (NTS_Common_Property.search_advanced_option)
                                    {
                                        //기간적용
                                        if (NTS_Common_Property.chk_date(_ims.CreationTime, _ims.LastAccessTime, _ims.LastWriteTime))
                                        {
                                            _normal_f_total_size += _ims.Length;

                                            // 생성일자 적용은 파일 카피할떄 적용
                                            // 파일리스트 작성                                                     
                                            _fc.wirte_normal_complete(f_total + " , " + f_name.Replace(",", " ") + " , " + f_ext + " , " + fn);
                                            f_total++;

                                        }
                                    }
                                    else
                                    {
                                        //기간 미적용
                                        _normal_f_total_size += _ims.Length;
                                        _fc.wirte_normal_complete(f_total + " , " + f_name.Replace(",", " ") + " , " + f_ext + " , " + fn);
                                        f_total++;

                                    }


                                    set_normal_alalysis("파일검색:" + f_total);


                                }



                                #endregion 전체파일 확보

                            }



                        }

                    }
                    catch (Exception ex)
                    {
                        //   Console.WriteLine("에러4" + ex.Message);rib
                        _fc.wirte_normal_complete_fase("확장자가 없는 파일 , " + f_name.Replace(",", " ") + " : " + currentDir);
                        //  Console.WriteLine("파일명:" + f_name);
                    }



                }
                catch (Exception ex)
                {
                    // ex.Message -> DB에 저장
                    //  DB_Controler.Access_Commander.insert_update_Normal_error(ex.Message + ":" + f_name + ":" + currentDir);
                    _fc.wirte_normal_complete_fase(ex.Message + " , " + f_name.Replace(",", " ") + ":" + currentDir);

                    // Console.WriteLine("에러5" + ex.Message);
                    continue;
                }
                //catch (UnauthorizedAccessException e) { continue;      }
                //catch (System.IO.DirectoryNotFoundException e) { continue;  }


                foreach (string str in subDirs) dirs.Push(str);
            }



        }

        private void File_Sever_UPLoad()
        {
            int fc_cnt = 0;
            long t = 0;


            DateTime SDt;
            DateTime EDt;


            // DataTable _tot_all = DB_Controler.Access_Commander.QueryExecute("SELECT * from file_list  ");

            DataTable _tot_all = _fc.read_normal_complete();


            string copy_folder = "";

            if (copy_path.Text.Equals("Path:"))
            {
                NTS_Common_Property.copy_position = System.IO.Directory.GetCurrentDirectory();
            }
            else
            {
                NTS_Common_Property.copy_position = copy_path.Text;
            }
            copy_folder += "_" + txt_company.Text.Trim();
            copy_folder += "_" + txt_depart.Text.Trim();
            copy_folder += "_" + cb_position.Text.Trim();
            copy_folder += "_" + txt_pcuser.Text.Trim();

            NTS_Common_Property.copy_position = NTS_Common_Property.copy_position + copy_folder; // copy_position 저장위치가 없으면 현재위치 아니면 설정 경로



            string fileN = "";
            foreach (DataRow row in _tot_all.Rows)
            {

                //  Console.WriteLine(fc_cnt + ":");

                fileN = row[3].ToString().Replace(",", "");

                FileInfo fs = new FileInfo(fileN);

                //  file_copy_bar2.Properties.Maximum = (int)fs.Length;

                try
                {

                    MessageBox.Show("===1===");
                    // 용량 제한 확인
                    if (NTS_Common_Property.limit && (fs.Length > NTS_Common_Property.M * 1024 * 1024))
                    {
                        // DB_Controler.Access_Commander.insert_update_copy_Normal_error(row[0].ToString(), "용량제한", row[3].ToString());

                        _fc.wirte_normal_copy_complete_fase("용량제한" + " , " + fileN);
                        continue;
                    }


                    TcpClient tcpClient = new TcpClient();
                    tcpClient.Connect(NTS_Common_Property.server_ip, Convert.ToInt32(NTS_Common_Property.server_port));
                    NetworkStream networkStream = tcpClient.GetStream();


                    string destFile = copy_folder + "\\" + fileN.Replace(":", "");



                    try
                    {
                        if (networkStream.CanWrite && networkStream.CanRead)
                        {


                            Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("0|" + destFile);
                            networkStream.Write(sendBytes, 0, sendBytes.Length);

                            byte[] ReadByte;
                            ReadByte = new byte[tcpClient.ReceiveBufferSize];
                            int BytesRead = networkStream.Read(ReadByte, 0, (int)ReadByte.Length);
                            string serverFileName = System.Text.Encoding.UTF8.GetString(ReadByte, 0, BytesRead);
                            // textBox1.Text += "*** 서버측 경로 " + serverFileName + "으로 업로드를 시작합니다.\r\n"; ;


                            /*파일 사이즈를 클라이언트로 전달*/
                            networkStream.Write(System.Text.Encoding.UTF8.GetBytes(fs.Length.ToString()), 0, System.Text.Encoding.UTF8.GetBytes(fs.Length.ToString()).Length);

                            /*클라이언트 측에서 준비되었는지 확인하고 준비되었다면 파일전송*/
                            int BytesRead2 = 0;
                            byte[] ConfirmByte = new byte[tcpClient.ReceiveBufferSize];
                            BytesRead2 = networkStream.Read(ConfirmByte, 0, (int)ConfirmByte.Length);


                            //  SDt = DateTime.Now;


                            byte[] _BUFFER = new byte[1024 * 10];

                            int nCurrentReadSizeOfBuffer = -1;

                            int readtot = 0;
                            using (FileStream fsOrigin = new FileStream(fileN, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                MessageBox.Show("fsOrigin :" + fsOrigin);
                                // int vv = 0;
                                //  Int64 tvv = 0;
                                using (BinaryReader brOrigin = new BinaryReader(fsOrigin))
                                {
                                    // NetworkStream nsOrigin = _File_Data.NetworkStreamEx;

                                    while ((nCurrentReadSizeOfBuffer = brOrigin.Read(_BUFFER, 0, _BUFFER.Length)) > 0)
                                    {
                                        networkStream.Write(_BUFFER, 0, nCurrentReadSizeOfBuffer);
                                        readtot = readtot + nCurrentReadSizeOfBuffer;
                                        // EDt = DateTime.Now;

                                        // tvv += nCurrentReadSizeOfBuffer;
                                        // vv = (int)(tvv * 100 / fs.Length);
                                        // gridfileCopyView.SetRowCellValue(fc_cnt, "진행", vv);
                                        //  if ((decimal)(EDt - SDt).TotalMilliseconds != 0) lb_speed.Text = ((tvv / (decimal)(EDt - SDt).TotalMilliseconds) / 1024).ToString("#,#0.#0") + "MB/S";

                                        //f/ile_copy_bar2.Position = vv;

                                        //Application.DoEvents();



                                        //-------------------------------------- 살려야한다  set_normal_server_upload(readtot + ":detail:" + fs.Length);


                                    }

                                    networkStream.Flush();
                                }
                                // fsOrigin.Flush();

                                fsOrigin.Close();
                            }


                            // if (chk_copy.Checked == true) gridfileCopyView.SetRowCellValue(fc_cnt, "진행", 100);
                            Array.Clear(_BUFFER, 0, 1024 * 10);

                        }
                    }
                    catch (SocketException)
                    {
                        //MessageBox.Show("서버접속에 실패하였습니다1.");
                    }
                    catch (System.IO.IOException)
                    {
                        //MessageBox.Show("서버접속에 실패하였습니다2.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        t++;
                        // 전송오류 정보
                        //FileInfo fi = new FileInfo(fileN);
                        //long filesize = fi.Length;
                        //string ext = fi.Extension.Replace(".", "").ToUpper();
                        //string filetime = fi.CreationTime.ToString();


                    }
                    finally
                    {

                        networkStream.Flush();
                        networkStream.Close();

                        tcpClient.Close();

                        fc_cnt++;

                        set_normal_server_upload(fc_cnt + ":total:" + _tot_all.Rows.Count);

                        FileInfo info = new FileInfo(fileN);
                        //long filesize = fi.Length;
                        //string ext = fi.Extension.Replace(".", "").ToUpper();
                        //string filetime = fi.CreationTime.ToString();

                        _md5 = "";
                        if (NTS_Common_Property.ck_md5)
                        {
                            _md5 = getFilesMD5Hash_one(@row[3].ToString());
                        }

                        // DB_Controler.Access_Commander.insert_update_copy_Normal(row, info.CreationTime.ToString(), info.LastWriteTime.ToString(), info.LastAccessTime.ToString(), _md5);

                        _fc.wirte_normal_copy_complete(
                             row[0] + " , " +
                             row[1].ToString().Replace(",", "") + " , " +
                             row[2] + " , " +
                             row[3].ToString().Replace(",", "") + " , " +
                              info.CreationTime.ToString() + " , " + info.LastWriteTime.ToString() + " , " + info.LastAccessTime.ToString() + " , " + _md5
                             );

                    }


                }

                catch (Exception ex)
                {

                    continue;

                }



            }


            //  DB_Controler.Access_Commander.Close_conn();

            _fc.close_all();
            set_normal_server_upload(fc_cnt + ":complete:" + _tot_all.Rows.Count);


        }

        public delegate void set_normal_mode_FileSystem_server_upload(string cou);
        private void set_normal_server_upload(string _p)
        {
            if (progressBarControl1.InvokeRequired)
            {
                try
                {
                    set_normal_mode_FileSystem_server_upload dr = new set_normal_mode_FileSystem_server_upload(set_normal_server_upload);
                    this.Invoke(dr, new object[] { _p });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {

                string[] _Data = _p.Split(':');

                if (_Data[1].Equals("detail"))
                {
                    progressBarControl1.Properties.Maximum = int.Parse(_Data[2]);
                    progressBarControl1.Position = int.Parse(_Data[0]);
                }
                else if (_Data[1].Equals("complete"))
                {

                    progressBarControl1.EditValue = 0;

                    show_result();

                    //   Files_Get_Start.Enabled = true;
                    start_button_init_on();
                }
                else
                {
                    try
                    {
                        progressBarControl1.Properties.Maximum = int.Parse(_Data[2]);
                        progressBarControl1.Position = int.Parse(_Data[0]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //    Application.DoEvents();




            }


        }

        public void show_footprinting()
        {

            this.Invoke(new Action(delegate ()
            {
                Process_State.Text = "";
                m_click("사전분석");
                // top_Menu7.Visible = true;
            }));



        }

        // 20230712 한민정 추가
        public void show_indexing()
        {

            this.Invoke(new Action(delegate ()
            {
                Index_state.Text = "";
                m_click("인덱싱");
            }));



        }



        public void drive_list_Net()
        {

            //  treeList2.Nodes.Clear();

            // TreeListNode nCom = treeList2.AppendNode(new object[] { "컴퓨터(" + Environment.MachineName + ")" }, null);

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    if (drive.DriveType.ToString().IndexOf("Net") > -1)
                    {
                        string drive_info = drive.ToString() + " (" + NTS_Common_Property.getSize(drive.TotalSize) + ") " + drive.DriveType.ToString();
                        //  drive_name.Caption = drive_info;

                        TreeListNode nDrive = treeList1.AppendNode(new object[] { drive_info, NTS_Common_Property.getSize(drive.TotalSize - drive.AvailableFreeSpace), drive.DriveType }, null);
                        if (!Regex.IsMatch(drive_info.Substring(0, 1), System.IO.Directory.GetCurrentDirectory().Substring(0, 1))) nDrive.Checked = false;
                        nDrive.StateImageIndex = 0;
                    }

                }
            }
            //  nCom.Expanded = true;

        }

        public void drive_list()
        {


            treeList1.Nodes.Clear();

            TreeListNode nCom = treeList1.AppendNode(new object[] { "컴퓨터(" + Environment.MachineName + ")" }, null);

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    string drive_info = drive.ToString() + " (" + NTS_Common_Property.getSize(drive.TotalSize) + ") " + drive.DriveType.ToString();
                    //  drive_name.Caption = drive_info;

                    TreeListNode nDrive = treeList1.AppendNode(new object[] { drive_info, NTS_Common_Property.getSize(drive.TotalSize - drive.AvailableFreeSpace), drive.DriveType }, nCom);
                    if (!Regex.IsMatch(drive_info.Substring(0, 1), System.IO.Directory.GetCurrentDirectory().Substring(0, 1))) nDrive.Checked = true;
                    nDrive.StateImageIndex = 0;


                }



            }
            nCom.Expanded = true;

        }


        public void show_alert(string _title, string _contents)
        {
            Alert_Form My_MessageBox = new Alert_Form(_title, _contents);
            My_MessageBox.StartPosition = FormStartPosition.Manual;
            My_MessageBox.Location = new Point(this.Location.X + ((this.Width / 2) - (My_MessageBox.Width / 2)), this.Location.Y + ((this.Height / 2) - (My_MessageBox.Height / 2)));
            My_MessageBox.ShowDialog();
        }

        public void show_alert_big(string _title, string _contents)
        {
            Alert_Form_big My_MessageBox = new Alert_Form_big(_title, _contents);
            My_MessageBox.StartPosition = FormStartPosition.Manual;
            My_MessageBox.Location = new Point(this.Location.X + ((this.Width / 2) - (My_MessageBox.Width / 2)), this.Location.Y + ((this.Height / 2) - (My_MessageBox.Height / 2)));
            My_MessageBox.ShowDialog();
        }

        public void show_alertYN(string _title, string _contents)
        {
            Alert_Form_YN My_MessageBox = new Alert_Form_YN(_title, _contents);
            My_MessageBox.StartPosition = FormStartPosition.Manual;
            My_MessageBox.Location = new Point(this.Location.X + ((this.Width / 2) - (My_MessageBox.Width / 2)), this.Location.Y + ((this.Height / 2) - (My_MessageBox.Height / 2)));
            My_MessageBox.ShowDialog();

        }


        public void show_alertNY(string _title, string _contents)
        {
            Alert_Form_NY My_MessageBox = new Alert_Form_NY(_title, _contents);
            My_MessageBox.StartPosition = FormStartPosition.Manual;

            My_MessageBox.Location = new Point(this.Location.X + ((this.Width / 2) - (My_MessageBox.Width / 2)), this.Location.Y + ((this.Height / 2) - (My_MessageBox.Height / 2)));
            My_MessageBox.ShowDialog();


        }

        //////////////////////////////////  풋프린팅 ////////////////////////////////////////////////////////

        #region


        #endregion

        FileSystemWatcher watcher = null;


        public void kill_foot_printing()
        {
            watcher.Created -= new FileSystemEventHandler(changed);
            watcher.Changed -= new FileSystemEventHandler(changed2);
            watcher.EnableRaisingEvents = false;
            watcher = null;

            Process[] proce = null;
            if (Is64bitMode())
            {
                //proce = Process.GetProcessesByName("Footprinting_64.exe");
                proce = Process.GetProcessesByName("FootprintingStandalone_64.exe");
            }
            else
            {
                //proce = Process.GetProcessesByName("Footprinting_32.exe");
                proce = Process.GetProcessesByName("FootprintingStandalone_32.exe");
            }

            Process current_Pro = Process.GetCurrentProcess();
            foreach (Process proc in proce)
            {
                if (proc.Id != current_Pro.Id)
                {
                    proc.Kill();
                }
            }


            //////////////////////////////////////////////////////


            if (Is64bitMode())
            {
                //proce = Process.GetProcessesByName("Footprinting_64");
                proce = Process.GetProcessesByName("FootprintingStandalone_64");
            }
            else
            {
                //proce = Process.GetProcessesByName("Footprinting_32");
                proce = Process.GetProcessesByName("FootprintingStandalone_32");
            }

            current_Pro = Process.GetCurrentProcess();
            foreach (Process proc in proce)
            {
                if (proc.Id != current_Pro.Id)
                {
                    proc.Kill();
                }
            }






            //FileInfo _fi = new FileInfo(Application.StartupPath + @"\Footprinting\HTML_Export\FootPrinting.htm");
            FileInfo _fi = new FileInfo(Application.StartupPath + @"\FootprintingStandalone\Footprinting\HTML_Export\FootPrinting.htm");
            //FileInfo _fi = new FileInfo(Application.StartupPath + @"C:\Footprinting\HTML_Export\FootPrinting.htm");


            if (_fi.Exists)
            {
                _fi.Delete();
            }

            FoorPrinting_State("사전분석이 취소 되었습니다.");

        }

        // 20230627 한민정 추가
        private void OnTimerTick(object sender, EventArgs e)
        {
            UpdateFromFile();
        }

        // 20230627 한민정 추가
        private void UpdateFromFile()
        {
            string filePath = Application.StartupPath + @"\FootprintingStandalone\Footprinting\progress.txt";
            string fileEnd = Application.StartupPath + @"\FootprintingStandalone\Footprinting\log.txt";
            // txt 파일이 Unicode로 작성되어 있음, Encoding Unicode 필요

            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                // fileInfo 객체 생성 확인
                if (fileInfo == null)
                {
                    return;
                }
                // 파일의 마지막 수정 시간이 현재 시간보다 이전이면 읽지 않음
                else if (fileInfo.LastWriteTime < DateTime.Now.AddMinutes(-1))
                {
                    return;
                }
                else
                {
                    string contentProgress = File.ReadAllText(filePath, Encoding.Unicode);
                    string contentLog = File.ReadAllText(filePath, Encoding.Unicode);

                    if (contentLog.Contains("##### Footprinting Standalone End #####"))
                    {
                        label7.Text = "";
                        label7.Visible = false;
                        bottom_Base.Visible = false;
                    }
                    else
                    {
                        label7.Text = contentProgress;
                        label7.Visible = true;
                    }

                }

            }
        }

        private void foot_printing_start()
        {
            watcher = new FileSystemWatcher();
            DriveInfo[] drives = DriveInfo.GetDrives();
            StringBuilder argumentsBuilder = new StringBuilder();

            if (NTS_Common_Property.search_mode == 1)
            {
                // MessageBox.Show("일반모드에서는 지원하지 않습니다.");
                show_alert("시스템 정보", "사전분석은" + Environment.NewLine + "일반모드에서는 지원하지 않습니다.");
                return;
            }

            FoorPrinting_State("사전분석 시작");

            watcher.Path = Application.StartupPath + @"\FootprintingStandalone\Footprinting";
            //  감시할 항목 설정 -> 파일생성,크기,이름 
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            //모든유형 감시 watcher.filter ="*.*"
            watcher.IncludeSubdirectories = true;// 하위 디렉토리 감시

            watcher.Created += new FileSystemEventHandler(changed);
            watcher.Changed += new FileSystemEventHandler(changed2);
            //감시 시작
            watcher.EnableRaisingEvents = true;

            //   FoorPrinting_State("폴더감시 시작:" + watcher.Path);


            Thread th = new Thread(() =>
            {
                //FoorPrinting_State("국보연 시작" );
                ProcessStartInfo start = new ProcessStartInfo();
                if (Is64bitMode())
                {
                    // 20230622 한민정 수정 Footprinting_64.exe 파일은 실행 안됨, 우선 FootprintingStandalone_64.exe로 실행
                    //start.FileName = Application.StartupPath + @"\Footprinting\Footprinting_64.exe";
                    start.FileName = Application.StartupPath + @"\FootprintingStandalone\Footprinting\FootprintingStandalone_64.exe";
                }
                else
                {
                    //start.FileName = Application.StartupPath + @"\Footprinting\Footprinting_32.exe";
                    start.FileName = Application.StartupPath + @"\FootprintingStandalone\Footprinting\FootprintingStandalone_32.exe";
                }
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                // start.WindowStyle = ProcessWindowStyle.Hidden;
                start.CreateNoWindow = true;

                // 20230622 한민정 수정 드라이버 검색 후 검색 한 모든 드라이버 대상으로 사전분석(Footprinting) 시작
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady == true)
                    {
                        string driveName = drive.Name.TrimEnd('\\');
                        argumentsBuilder.Append(" \\\\.\\").Append(driveName);
                    }
                }
                start.Arguments += argumentsBuilder.ToString();

                Process proce = Process.Start(start);
                StreamReader res = proce.StandardOutput;

                //StreamWriter writer;
                //writer = File.CreateText(Application.StartupPath  + @"\error.txt");
                //writer.WriteLine(res.ReadToEnd());
                //writer.Close();

            });

            th.IsBackground = true;    // 메인 종료시 같이 종료

            th.Start();
            chk_indexing = true;
        }


        // 20230712 한민정 추가
        
        private void indexing_start()
        {
            watcher = new FileSystemWatcher();
            StringBuilder argumentsBuilder = new StringBuilder();

            string filePath = Application.StartupPath + @"\FootprintingStandalone\Footprinting\indexing_result.txt";
            FileInfo fileInfo = new FileInfo(filePath);

            show_alert("진행정보", "인덱싱을 시작합니다.");

            if (NTS_Common_Property.search_mode == 1)
            {
                show_alert("시스템 정보", "인덱싱은" + Environment.NewLine + "일반모드에서는 지원하지 않습니다.");
                return;
            }

            try
            {
                string pythonExecutablePath = Application.StartupPath + @"\FootprintingStandalone\Footprinting\indexing.exe";

                Process process = new Process();
                process.StartInfo.FileName = pythonExecutablePath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                StringBuilder outputBuilder = new StringBuilder();

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputBuilder.AppendLine(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();

                string output = outputBuilder.ToString();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                {
                    show_alert("오류 발생", error);
                }

                if (!string.IsNullOrEmpty(output))
                {
                    string[] lines = output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {

                        if (line == "경로가 존재하지 않습니다. 사전 분석을 먼저 진행 해주세요.")
                        {
                            show_alert("진행정보", "인덱싱 할 파일이 존재하지 않습니다.\n 사전 분석을 먼저 진행 해주세요");
                            return;
                        }

                        //show_alert("진행정보", line);
                        //Index_state.Text += line;

                        Invoke(new Action(() => Index_state.Text = line));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.FullPath) { UseShellExecute = true });
        }


        private bool Is64bitMode()
        {
            //  Console.WriteLine(Environment.);
            //return System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8; // 해당 파 64인지 32인지 확인 4-> 32bit 8-> 64bit


            if (Directory.Exists(@"C:\Windows\SysWOW64"))
            {
                return true;
            }
            else
            {
                return false;
            }



        }

        private void changed(object source, FileSystemEventArgs e)
        {

            readline_new(e.FullPath);
            //  File.Readline
        }

        private void changed2(object source, FileSystemEventArgs e)
        {

            readline_new2(e.FullPath);

        }

        // 20230712 한민정 추가
        private void changed3(object source, FileSystemEventArgs e)
        {

            readline_new3(e.FullPath);

        }

        Dictionary<string, string> _ims = new Dictionary<string, string>();

        private bool chk_data(string _line)
        {

            try
            {
                _ims.Add(_line.Substring(19, _line.Length - 19), _line.Substring(19, _line.Length - 19));
                return false;

            }
            catch (Exception ex)
            {
                return true;
            }


        }

        private void readline_new(string fname)
        {


            if (fname.IndexOf("log.txt") > -1)
            {
                var list = new List<string>();
                var rd = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                string line;


                using (var streamReader = new StreamReader(rd, System.Text.Encoding.Unicode))
                {




                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (!chk_data(line))
                        {
                            FoorPrinting_State(line.Substring(19, line.Length - 19));
                        }

                    }



                }

            }



        }

        private void readline_new2(string fname)
        {


            Console.WriteLine("--------변경----------");


            if (fname.IndexOf("log.txt") > -1)
            {
                var list = new List<string>();
                var rd = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                string line;


                using (var streamReader = new StreamReader(rd, System.Text.Encoding.Unicode))
                {




                    while ((line = streamReader.ReadLine()) != null)
                    {
                        // list.Add(line);
                        if (!chk_data(line))
                        {

                            FoorPrinting_State(line.Substring(19, line.Length - 19));

                            if (line.IndexOf("Footprinting Standalone End") > -1)
                            {
                                // label2.Text = "ok";
                                watcher.Created -= new FileSystemEventHandler(changed);
                                watcher.Changed -= new FileSystemEventHandler(changed2);
                                watcher = null;
                                show_footprinting();


                            }


                        }
                    }



                }

            }


        }
        // 20230712 한민정 추가
        private void readline_new3(string fname)
        {
            Console.WriteLine("--------변경----------");

            if (fname.IndexOf("indexing_result.txt") > -1)
            {
                var list = new List<string>();
                var rd = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                string line;

                using (var streamReader = new StreamReader(rd, System.Text.Encoding.Unicode))
                {
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (!chk_data(line))
                        {
                            Indexing_State(line.Substring(19, line.Length - 19));

                            if (line.IndexOf("Indexing document 26 of 26") > -1)
                            {
                                watcher.Created -= new FileSystemEventHandler(changed3);
                                watcher = null;
                                show_indexing();
                            }
                        }
                    }
                }
            }
        }


        //픗프린팅 진행상태
        public delegate void setFoorPrinting_State(string _p);
        private void FoorPrinting_State(string _p)
        {

            if (Process_State.InvokeRequired)
            {

                setFoorPrinting_State dr = new setFoorPrinting_State(FoorPrinting_State);
                this.Invoke(dr, new object[] { _p });
            }
            else
            {
                Process_State.Text = "사전분석:" + _p;

            }


        }


        //////////////////////////////////////////////////// Foot printing 종료 ////////////////////////////////

        // 20230711 한민정 추가 인덱싱 진행상태
        public delegate void setIndexing_State(string _p);
        private void Indexing_State(string _p)
        {
            if (Process_State.InvokeRequired)
            {
                setIndexing_State dr = new setIndexing_State(Indexing_State);
                this.Invoke(dr, new object[] { _p });
            }
            else
            {
                Index_state.Text = "인덱싱:" + _p;

            }

        }

        public void send_Message(string msg)
        {
            if (setEnable)
            {
                MessageBox.Show(msg);
            }
        }



        // 2022.04.08 물리이미징 드라이브 선택을 위한 코드

        public DevExpress.XtraTreeList.TreeList Get_Main_Tree()
        {
            return treeList1;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}