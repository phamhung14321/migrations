using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2013.Word;
using Person.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Person
{
    public partial class Form1 : Form
    {
        private SchoolContext dbContext;
        private BindingList<Student> students;
        private int selectedStudentId = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbContext = new SchoolContext(); 
            LoadStudents();  
        }
        private void LoadStudents()
        {
            var studentList = dbContext.Students.ToList();
            students = new BindingList<Student>(studentList); 
            dgvSinhVien.DataSource = students;
        }
        private bool CheckIdSinhVien(string idNewStudent)
        {
            int length = dgvSinhVien.Rows.Count;
            for (int i = 0; i < length; i++)
            {
                if (dgvSinhVien.Rows[i].Cells[0].Value != null)
                {
                    if (dgvSinhVien.Rows[i].Cells[0].Value.ToString() == idNewStudent)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddStudent(string fullName, int age, string major)
        {
            var newStudent = new Student
            {
                HoTen = fullName,
                Tuoi = age,
                Nganh = major
            };

            dbContext.Students.Add(newStudent);
            dbContext.SaveChanges();
            students.Add(newStudent);  // Thêm vào BindingList
        }

        // Hàm cập nhật sinh viên
        private void UpdateStudent(int studentId, string fullName, int age, string major)
        {

                var student = dbContext.Students.Find(studentId);
                if (student != null)
                {
                    student.HoTen = fullName;
                    student.Tuoi = age;
                    student.Nganh = major;

                    dbContext.SaveChanges();
                    LoadStudents(); 
                }

        }
        // Hàm xóa sinh viên
        private void DeleteStudent(int studentId)
        {
            var student = dbContext.Students.Find(studentId);
            if (student != null)
            {
                dbContext.Students.Remove(student);
                dbContext.SaveChanges();
                students.Remove(student);  // Xóa khỏi BindingList
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string fullName = txtHoTen.Text;  // txtFullName là TextBox cho tên sinh viên
            int age = int.Parse(txtTuoi.Text);     // txtAge là TextBox cho tuổi
            string major = cmbKhoa.SelectedItem.ToString(); // Sửa lại tên
                                                             

            AddStudent(fullName, age, major);

            // Xóa dữ liệu trong TextBox sau khi thêm
            txtHoTen.Clear();
            txtTuoi.Clear();
            cmbKhoa.SelectedIndex = 0;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
                if (selectedStudentId != -1)
                {
                    string fullName = txtHoTen.Text;
                    int age = int.Parse(txtTuoi.Text);
                    string major = cmbKhoa.SelectedItem.ToString();

                    // Gọi hàm cập nhật sinh viên
                    UpdateStudent(selectedStudentId, fullName, age, major);

                    // Xóa dữ liệu trong TextBox sau khi sửa
                    txtHoTen.Clear();
                    txtTuoi.Clear();
                    cmbKhoa.SelectedIndex = 0;
                    selectedStudentId = -1;  // Đặt lại sau khi sửa
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để sửa!");
                }
            }
        

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedStudentId >0)
            {
                // Xác nhận xóa sinh viên
                var result = MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteStudent(selectedStudentId);

                    // Xóa dữ liệu trong TextBox sau khi xóa
                    txtHoTen.Clear();
                    txtTuoi.Clear();
                    cmbKhoa.SelectedIndex = 0;
                    selectedStudentId = -1;  // Đặt lại sau khi xóa
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa!");
            }
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin sinh viên từ dòng đã chọn
                var selectedRow = dgvSinhVien.Rows[e.RowIndex].DataBoundItem as Student;

                if (selectedRow != null)
                {
                    // Hiển thị thông tin sinh viên lên các TextBox
                    selectedStudentId = selectedRow.MSSV;
                    txtHoTen.Text = selectedRow.HoTen;
                    txtTuoi.Text = selectedRow.Tuoi.ToString();
                    cmbKhoa.SelectedItem = selectedRow.Nganh;
                }
            }
        }
    }
}
