using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication9//cpu_max evaluation
{

    enum cellstatus//�񋓌^�̐錾
    {
        nothing, black, white, cpu
    }

    public partial class Form1 : Form
    {
        //�ʒu�A��Ԃ̐錾�i�t�B�[���h�j
        int[,] cells = new int[10, 10];

        int[,] cellcpu = new int[10, 10];

        int[,] undocells = new int[10, 10];

        int[,] undocellcpu = new int[10, 10];

        int[,] ecells = new int[10, 10];

        int turn = (int)cellstatus.black;

        int max = -200;

        private Timer t;
        
        public Form1()
        {
            InitializeComponent();
            Text = "Othello";
            BackColor = Color.Green;

            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    cellcpu[x , y] = -200;
                }
            }

            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    ecells[x, y] = -1;
                }
            }

            ecells[1, 1] = 30;
            ecells[8, 1] = 30;
            ecells[1, 8] = 30;
            ecells[8, 8] = 30;
            ecells[2, 1] = -12;
            ecells[7, 1] = -12;
            ecells[1, 2] = -12;
            ecells[8, 2] = -12;
            ecells[1, 7] = -12;
            ecells[8, 7] = -12;
            ecells[2, 8] = -12;
            ecells[7, 8] = -12;
            ecells[2, 2] = -15;
            ecells[7, 2] = -15;
            ecells[2, 7] = -15;
            ecells[7, 7] = -15;
            ecells[3, 2] = -3;
            ecells[4, 2] = -3;
            ecells[5, 2] = -3;
            ecells[6, 2] = -3;
            ecells[2, 3] = -3;
            ecells[2, 4] = -3;
            ecells[2, 5] = -3;
            ecells[2, 6] = -3;
            ecells[7, 3] = -3;
            ecells[7, 4] = -3;
            ecells[7, 5] = -3;
            ecells[7, 6] = -3;
            ecells[3, 7] = -3;
            ecells[4, 7] = -3;
            ecells[5, 7] = -3;
            ecells[6, 7] = -3;
            ecells[3, 1] = 0;
            ecells[3, 8] = 0;
            ecells[6, 8] = 0;
            ecells[6, 1] = 0;
            ecells[1, 3] = 0;
            ecells[3, 3] = 0;
            ecells[6, 3] = 0;
            ecells[8, 3] = 0;
            ecells[1, 6] = 0;
            ecells[3, 6] = 0;
            ecells[6, 6] = 0;
            ecells[8, 6] = 0;

            board.show_initial(label2, label3, label1, label4, label5, cells, undocells);

            board.show_board(pictureBox1, cells);          
        }


        private void pictureBox1_click(object sender, EventArgs e)
        {
            label4.Text = " ";

            int canputb1 = board.show_count(cells, (int)cellstatus.white);//count���\�b�h�i���j
            int canputw1 = board.show_count(cells, (int)cellstatus.black);//count���\�b�h�i���j

            if (canputb1 == 0 && canputw1 == 0)//���ʕ\��
            {
                board.show_result(label3, label1, cells);
                goto terminate;
            }

//����
                if (turn == (int)cellstatus.black)
                {
                    cpu.undo_record(cells, undocells);//�Ֆʂ̈ꎞ�I�ȕۑ�

                    int canputb = board.show_count(cells, (int)cellstatus.white);//count���\�b�h�i���j

                    //label5.Text = canputb.ToString();//�łĂ�}�X�ڂ̐�

                    if (canputb == 0)
                    {
                        //���̔�
                        MessageBox.Show("�łĂ�ꏊ������܂���B");
                        label2.Text = "���̔Ԃł��B";
                        turn = (int)cellstatus.white;
                        goto end;
                    }

                    //�}�E�X���W�̎擾
                    System.Drawing.Point sp = System.Windows.Forms.Cursor.Position;
                    System.Drawing.Point cp = this.PointToClient(sp);
                    int dx = cp.X - 12;
                    int dy = cp.Y - 42;

                    int checknull = 0;
                    int p = board.turn_over(cells, (int)cellstatus.white, dx, dy, label2, checknull);//turnover���\�b�h�i���j

                    if (p == 0)
                    {
                        label4.Text = "�����ɂ͒u���܂���B";
                        goto end1;
                    }
                    else if (p == 2)
                    {
                        label2.Text = "���̔Ԃł��B";
                        turn = (int)cellstatus.white;
                    }
                    else
                    {
                        goto end1;
                    }

                    board.show_board(pictureBox1, cells);//show_board���\�b�h

                    board.show_cresult(label3, label1, cells);//���󌋉ʕ\��

                    int canputw = board.show_count(cells, (int)cellstatus.black);//count���\�b�h�i���j

                    //label5.Text = canputw.ToString();

                end: ; 

                    t = new Timer();
                    t.Tick += new EventHandler(cpu_put);
                    t.Interval = 500; // �~���b�P�ʂŎw��
                    t.Start();

                end1: ;

                }

//����
                /*else if (turn == (int)cellstatus.white)
                {
                    //cpu.undo_record(cells, undocells);//�Ֆʂ̈ꎞ�I�ȕۑ�

                    int canputw = board.show_count(cells, (int)cellstatus.black);//count���\�b�h�i���j

                    label5.Text = canputw.ToString();//�łĂ�ꏊ�̃J�E���g��\������

                    if (canputw == 0)//�p�X�̏���
                    {
                        //���̔�
                        MessageBox.Show("�łĂ�ꏊ������܂���B");
                        label2.Text = "���̔Ԃł��B";
                        turn = (int)cellstatus.black;
                        goto end;
                    }

                    int cto = 0;

                    int r = cpu.cpu_put(cells, (int)cellstatus.black, cto, cellcpu, max, undocellcpu, ecells);//cpu���\�b�h�i���j

                    if (r == 1)
                    {
                        //���̔�
                        label2.Text = "���̔Ԃł��B";
                        turn = (int)cellstatus.black;
                        
                    }

                    board.show_board(pictureBox1, cells);//�Ֆʕ\��

                    board.show_cresult(label3, label1, cells);//���󌋉ʕ\��

                    int canputb = board.show_count(cells, (int)cellstatus.white);//count���\�b�h�i���j

                    label5.Text = canputb.ToString();//�łĂ�}�X�ڂ̐�

                end: ;
                }//����*/

       terminate: ;
            }//�N���b�N�C�x���g



        private void �I��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ���j���[ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.show_initial(label2, label3, label1, label4, label5, cells, undocells);

            turn = (int)cellstatus.black;

            board.show_board(pictureBox1, cells);
        }

        private void ��蒼��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int un = cpu.undo_turnover(cells, undocells);

            board.show_board(pictureBox1, cells);

            if (un == 1 && turn == (int)cellstatus.white)
            {
                label2.Text = "���̔Ԃł��B";
                turn = (int)cellstatus.black;

            }

        }

        private void cpu_put(object sender, EventArgs e)
        {
            t.Stop();

            label4.Text = " ";

            int canputb1 = board.show_count(cells, (int)cellstatus.white);//count���\�b�h�i���j
            int canputw1 = board.show_count(cells, (int)cellstatus.black);//count���\�b�h�i���j

            if (canputb1 == 0 && canputw1 == 0)//���ʕ\��
            {
                board.show_result(label3, label1, cells);
                goto end;
            }

            //cpu.undo_record(cells, undocells);//�Ֆʂ̈ꎞ�I�ȕۑ�

            int canputw = board.show_count(cells, (int)cellstatus.black);//count���\�b�h�i���j

            //label5.Text = canputw.ToString();//�łĂ�ꏊ�̃J�E���g��\������

            if (canputw == 0)//�p�X�̏���
            {
                //���̔�
                MessageBox.Show("�łĂ�ꏊ������܂���B");
                label2.Text = "���̔Ԃł��B";
                turn = (int)cellstatus.black;
                goto end;
            }

            int cto = 0;

            int r = cpu.cpu_put(cells, (int)cellstatus.black, cto, cellcpu, max, undocellcpu, ecells);//cpu���\�b�h�i���j

            if (r == 1)
            {
                //���̔�
                label2.Text = "���̔Ԃł��B";
                turn = (int)cellstatus.black;

            }

            board.show_board(pictureBox1, cells);//�Ֆʕ\��

            board.show_cresult(label3, label1, cells);//���󌋉ʕ\��

            int canputb = board.show_count(cells, (int)cellstatus.white);//count���\�b�h�i���j

            label5.Text = canputb.ToString();//�łĂ�}�X�ڂ̐�

        end: ;

        } 

    }//form1�N���X

}//namespace

