using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication9
{
    class board
    {
        public static void show_board(PictureBox pic, int[,] cells)//�Ֆʂ̕\��
        {
            Bitmap canvas1 = new Bitmap(pic.Width, pic.Height);
            Graphics g1 = Graphics.FromImage(canvas1);
            Graphics g2 = Graphics.FromImage(canvas1);
            for (int c = 0; c <= 8; c++)
            {
                g1.DrawLine(Pens.Black, c * 50, 0, c * 50, 400);
            }

            for (int d = 0; d <= 8; d++)
            {
                g1.DrawLine(Pens.Black, 0, d * 50, 400, d * 50);
            }

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (cells[i, j] == (int)cellstatus.black)
                    {
                        g2.FillEllipse(Brushes.Black, i * 50 - 40, j * 50 - 40, 30, 30);
                    }
                    else if (cells[i, j] == (int)cellstatus.white)
                    {
                        g2.FillEllipse(Brushes.White, i * 50 - 40, j * 50 - 40, 30, 30);
                    }
                }
            }

            g1.Dispose();
            g2.Dispose();
            pic.Image = canvas1;
        }//show_board���\�b�h


        public static void show_initial(Label label2, Label label3, Label label1, Label label4, Label label5, int[,] cells, int[,] undocells)//������Ԃ̏�������
        {
            label2.Text = "���̔Ԃł��B";
            label3.Text = "��";
            label1.Text = "��";
            label4.Text = " ";
            label5.Text = " ";

            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    cells[x, y] = (int)cellstatus.nothing;
                }
            }

            cells[4, 4] = (int)cellstatus.white;
            cells[5, 4] = (int)cellstatus.black;
            cells[4, 5] = (int)cellstatus.black;
            cells[5, 5] = (int)cellstatus.white;

            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    undocells[x, y] = cells[x, y];
                }
            }

        }//show_initial���\�b�h


        public static void show_result(Label label3, Label label1, int[,] cells)
        {
            int countb = 0;
            int countw = 0;
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    if (cells[i, j] == (int)cellstatus.black)
                    {
                        countb++;
                    }
                    else if (cells[i, j] == (int)cellstatus.white)
                    {
                        countw++;
                    }
                }
            }

            label3.Text = "��" + countb.ToString();
            label1.Text = "��" + countw.ToString();

            if (countb < countw)
            {
                MessageBox.Show("���̏����ł��B");
            }
            else if (countw < countb)
            {
                MessageBox.Show("���̏����ł��B");
            }
            else if (countb == countw)
            {
                MessageBox.Show("���������ł��B");
            }

        }//show_result���\�b�h


        public static void show_cresult(Label label3, Label label1, int[,] cells)
        {
            int countb = 0;
            int countw = 0;
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    if (cells[i, j] == (int)cellstatus.black)
                    {
                        countb++;
                    }
                    else if (cells[i, j] == (int)cellstatus.white)
                    {
                        countw++;
                    }
                }
            }

            label3.Text = "��" + countb.ToString();
            label1.Text = "��" + countw.ToString();
        }//show_cresult���\�b�h


        public static int show_count(int[,] cells, int turn)
        {
            //�R�}���u����ꏊ�̃J�E���g
            int canput = 0;
            for (int x = 1; x <= 8; x++)//�S���ڂ̏����擾����B
            {
                for (int y = 1; y <= 8; y++)
                {
                    if (cells[x, y] == (int)cellstatus.nothing)
                    {
                        for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//��Ԗڂ̏���
                                {
                                    for (int i = 2; i <= 8; i++)//��Ԗڈȍ~�̏���
                                    {
                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.white:
                                                if (turn == (int)cellstatus.black)
                                                {
                                                    canput++;
                                                    goto a;
                                                }
                                                else if (turn == (int)cellstatus.white)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.black)
                                                {
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.white)
                                                {
                                                    canput++;
                                                    goto a;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.nothing:
                                                goto a;
                                        }
                                    }
                                }
                            a: ;
                            }
                        }
                    }
                }
            }

            return canput;

        }//show_count���\�b�h


        public static int turn_over(int[,] cells, int turn, int dx, int dy, Label label2, int checknull)
        {
            //���ڏ�Ԃ̍X�V�i���j(put_black���\�b�h)
            for (int x = 1; x <= 8; x++)//���ڂ̈ʒu���擾���郋�[�v
            {
                for (int y = 1; y <= 8; y++)
                {
                    Rectangle cellxy = new Rectangle(x * 50 - 50, y * 50 - 50, 50, 50);
                    if (cellxy.Contains(dx, dy))
                    {
                        //���łɃR�}���u����Ă���ꏊ�ɂ͑łĂȂ�
                        if (cells[x, y] == (int)cellstatus.white || cells[x, y] == (int)cellstatus.black)
                        {
                            return 0;
                        }

                        //�R�}�𗠕Ԃ����\�b�h(turn�͕ύX���Ȃ�)

                        for (int d = -1; d <= 1; d++)//8�����̏��ڏ����擾���郋�[�v
                        {
                            for (int f = -1; f <= 1; f++)
                            {
                                if (cells[x + 1 * d, y + 1 * f] == turn)//��Ԗڂ̏���
                                {
                                    int count = 1;

                                    for (int i = 2; i <= 8; i++)
                                    {
                                        switch (cells[x + i * d, y + i * f])
                                        {
                                            case (int)cellstatus.white:
                                                if (turn == (int)cellstatus.white)
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else if (turn == (int)cellstatus.black)
                                                {
                                                    checknull++;
                                                    cells[x, y] = (int)cellstatus.white;
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.white;
                                                    }
                                                    goto a;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.black:
                                                if (turn == (int)cellstatus.white)
                                                {
                                                    checknull++;
                                                    cells[x, y] = (int)cellstatus.black;
                                                    for (int z = 1; z <= count; z++)
                                                    {
                                                        cells[x + z * d, y + z * f] = (int)cellstatus.black;
                                                    }
                                                    goto a;
                                                }
                                                else if (turn == (int)cellstatus.black)
                                                {
                                                    count++;
                                                    break;
                                                }
                                                else
                                                {
                                                    goto a;
                                                }

                                            case (int)cellstatus.nothing:
                                                goto a;
                                        }
                                    }
                                }
                            a: ;
                            }
                        }
                    }
                }
            }
            if (checknull == 0)
            {
                return 1;
            }
            else if (checknull != 0)
            {
                return 2;
            }
            else
            {
                return 1;
            }

        }//turn_over���\�b�h

    }//board�N���X
}//namespace�@
