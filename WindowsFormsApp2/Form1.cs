using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Models;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtmssv_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Model1 model = new Model1();
                List<SinhVien> SinhViens = model.SinhViens.ToList();
                List<Faculty> faculties = model.Faculties.ToList();
                FillFalcultyCombobox(faculties);
                BindGrid(SinhViens);
            }   
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFalcultyCombobox(List<Faculty> faculties)
        {
            this.cbmKhoa.DataSource = faculties;
            this.cbmKhoa.DisplayMember = "FacultyName";
            this.cbmKhoa.ValueMember = "FacltyID";
        }

        private void BindGrid(List<SinhVien> SinhViens)
        {
            dtvSinhVien.Rows.Clear();
            foreach(var item in SinhViens)
            {
                int index = dtvSinhVien.Rows.Add();
                dtvSinhVien.Rows[index].Cells[0].Value = item.StudentID;
                dtvSinhVien.Rows[index].Cells[1].Value = item.FullName;
                dtvSinhVien.Rows[index].Cells[2].Value = item.FacultyID;
                dtvSinhVien.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                ValidateInput();

                // Kiểm tra xem mã số sinh viên đã tồn tại hay chưa
                int selectedRow = GetSelectedRow(txtmssv.Text);
                if (selectedRow == -1)
                {
                    // Thêm mới nếu không tìm thấy
                    selectedRow = dtvSinhVien.Rows.Add();
                    MessageBox.Show("Thêm sinh viên mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Nếu tìm thấy, cập nhật dữ liệu
                    MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Cập nhật hoặc thêm dữ liệu
                InsertUpdate(selectedRow);

                // Reset các ô nhập liệu và combobox về trạng thái mặc định
                ResetInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetSelectedRow(string studentID)
        {
            for (int i = 0; i < dtvSinhVien.Rows.Count; i++)
            {
                if (dtvSinhVien.Rows[i].Cells[0].Value != null &&
                    !string.IsNullOrEmpty(dtvSinhVien.Rows[i].Cells[0].Value.ToString()))
                {
                    if (dtvSinhVien.Rows[i].Cells[0].Value.ToString() == studentID)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void ValidateInput()
        {
            // Kiểm tra các trường nhập liệu có để trống không
            if (string.IsNullOrWhiteSpace(txtmssv.Text) ||
                string.IsNullOrWhiteSpace(txthoten.Text) ||
                string.IsNullOrWhiteSpace(txtdiem.Text))
            {
                throw new Exception("Vui lòng nhập đầy đủ thông tin sinh viên.");
            }

            // Kiểm tra định dạng mã sinh viên
            if (!IsValidStudentID(txtmssv.Text))
            {
                throw new Exception("Mã số sinh viên không hợp lệ. Vui lòng nhập đúng định dạng (10 chữ số).");
            }

            // Kiểm tra định dạng tên sinh viên
            if (!IsValidFullName(txthoten.Text))
            {
                throw new Exception("Tên sinh viên không hợp lệ. Tên phải là chữ, độ dài từ 3 đến 100 ký tự, không chứa ký tự đặc biệt.");
            }

            // Kiểm tra điểm trung bình có phải số hợp lệ và nằm trong khoảng 0-10
            if (!float.TryParse(txtdiem.Text, out float dtb) || dtb < 0 || dtb > 10)
            {
                throw new Exception("Điểm trung bình sinh viên không hợp lệ. Vui lòng nhập số thập phân từ 0 đến 10.");
            }
        }

        private bool IsValidStudentID(string studentID)
        {
            // Kiểm tra mã sinh viên có phải là chuỗi số và có đúng 10 ký tự
            return studentID.Length == 10 && studentID.All(char.IsDigit);
        }

        private bool IsValidFullName(string fullName)
        {
            // Kiểm tra độ dài tên từ 3 đến 100 ký tự
            if (fullName.Length < 3 || fullName.Length > 100)
                return false;

            // Kiểm tra tên chỉ chứa chữ cái và khoảng trắng
            foreach (char c in fullName)
            {
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            }
            return true;
        }

        private void InsertUpdate(int selectedRow)
        {
            // Thực hiện cập nhật dữ liệu vào bảng (khi dữ liệu đã được kiểm tra)
            dtvSinhVien.Rows[selectedRow].Cells[0].Value = txtmssv.Text;
            dtvSinhVien.Rows[selectedRow].Cells[1].Value = txthoten.Text;
            dtvSinhVien.Rows[selectedRow].Cells[3].Value = float.Parse(txtdiem.Text).ToString("0.00");
            dtvSinhVien.Rows[selectedRow].Cells[2].Value = cbmKhoa.Text;
        }

        private void ResetInputFields()
        {
            txtmssv.Clear();
            txthoten.Clear();
            txtdiem.Clear();
            if (cbmKhoa.Items.Count > 0)
            {
                cbmKhoa.SelectedIndex = 0; // Chọn khoa mặc định là khoa đầu tiên
            }
            txtmssv.Focus(); // Đặt con trỏ vào ô nhập mã sinh viên
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có dòng nào được chọn không
                if (dtvSinhVien.CurrentRow == null || dtvSinhVien.CurrentRow.Index == -1)
                {
                    throw new Exception("Vui lòng chọn một sinh viên để sửa.");
                }

                // Lấy chỉ số dòng được chọn
                int selectedRow = dtvSinhVien.CurrentRow.Index;

                // Xác nhận sửa
                DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn sửa thông tin sinh viên này?", "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    // Kiểm tra dữ liệu đầu vào
                    ValidateInput();

                    // Cập nhật dữ liệu trong DataGridView
                    InsertUpdate(selectedRow);

                    MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cập nhật các thông tin khác nếu cần
                    // UpdateTotalStudents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát chương trình?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có dòng nào được chọn không
                if (dtvSinhVien.CurrentRow == null || dtvSinhVien.CurrentRow.Index == -1)
                {
                    throw new Exception("Vui lòng chọn một sinh viên để xoá.");
                }

                // Lấy chỉ số dòng được chọn
                int selectedRow = dtvSinhVien.CurrentRow.Index;

                // Hỏi xác nhận từ người dùng
                DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xoá sinh viên này?", "Xác nhận xoá", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    // Xoá dòng được chọn
                    dtvSinhVien.Rows.RemoveAt(selectedRow);
                    MessageBox.Show("Xoá sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset các ô nhập liệu sau khi xoá
                    ResetInputFields();

                    // Cập nhật tổng số sinh viên nếu có chức năng này
                    // UpdateTotalStudents();
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu xảy ra lỗi
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.RowIndex >= 0 && e.RowIndex < dtvSinhVien.Rows.Count)
                {
                    DataGridViewRow selectRow = dtvSinhVien.Rows[e.RowIndex];
                    txtmssv.Text = selectRow.Cells[0].Value?.ToString();
                    txthoten.Text = selectRow.Cells[1].Value?.ToString();
                    cbmKhoa.Text = selectRow.Cells[2].Value?.ToString();
                    txtdiem.Text = selectRow.Cells[3].Value?.ToString();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


