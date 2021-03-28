using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Chess_Minimax
{
    class Piece
    {
        public string colour;
        public string code = " ";
        public string name = "";
        public double value;

        public Piece(string colour)
        {
            this.colour = colour;
        }

        public virtual void Format_Piece()
        {
            switch (colour)
            {
                case "W":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "B":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    break;
            }
        }

        public virtual string GetCode()
        {
            return code;
        }

        public virtual bool Piece_Check_Move(int piece_x, int piece_y, int final_x, int final_y, Piece[,] board)
        {
            return true;
        }
    }
    class Castle : Piece
    {
        public Castle(string colour, double value) : base(colour)
        {
            this.colour = colour;
            code = "c";
            this.value = value;
            name = "Castle";
        }

        public override bool Piece_Check_Move(int piece_x, int piece_y, int final_x, int final_y, Piece[,] board)
        {
            bool acceptable = false;

            acceptable = piece_x == final_x || piece_y == final_y;

            if (acceptable)
            {
                if (piece_x == final_x)
                {
                    if (piece_y < final_y)//DOWN
                    {
                        for (int i = piece_y + 1; i < final_y; i++)
                        {
                            if (board[piece_x, i] != null)
                            {
                                acceptable = false;
                            }
                        }
                    }
                    else//UP
                    {
                        for (int i = piece_y - 1; i > final_y; i--)
                        {
                            if (board[piece_x, i] != null)
                            {
                                acceptable = false;
                            }
                        }
                    }
                }
                else
                {
                    if (piece_x < final_x)//RIGHT 
                    {
                        for (int i = piece_x + 1; i < final_x; i++)
                        {
                            if (board[i, piece_y] != null)
                            {
                                acceptable = false;
                            }
                        }
                    }
                    else//LEFT
                    {
                        for (int i = piece_x - 1; i > final_x; i--)
                        {
                            if (board[i, piece_y] != null)
                            {
                                acceptable = false;
                            }
                        }
                    }
                }
            }

            return acceptable;
        }
    }
    class Pawn : Piece
    {
        public Pawn(string colour, double value) : base(colour)
        {
            this.colour = colour;
            code = "p";
            this.value = value;
            name = "Pawn";
        }

        public override bool Piece_Check_Move(int piece_x, int piece_y, int final_x, int final_y, Piece[,] board)
        {
            bool acceptable = false;

            if (colour == "W")
            {
                if (board[final_x, final_y] == null)
                {
                    if (piece_x == final_x)
                    {
                        if (piece_y == 6) // first Move
                        {
                            acceptable = final_y == piece_y - 1 || final_y == piece_y - 2;
                            if (acceptable && final_y == piece_y - 2)
                            {
                                acceptable = board[piece_x, piece_y - 1] == null;
                            }
                        }
                        else // standard Move
                        {
                            acceptable = final_y == piece_y - 1;
                        }
                    }
                }
                else
                {
                    acceptable = (final_x == piece_x - 1 || final_x == piece_x + 1) && final_y == piece_y - 1;
                }


            }
            else
            {
                if (board[final_x, final_y] == null)
                {
                    if (piece_x == final_x)
                    {
                        if (piece_y == 1) // first Move
                        {
                            acceptable = final_y == piece_y + 1 || final_y == piece_y + 2;
                            if (acceptable && final_y == piece_y + 2)
                            {
                                acceptable = board[piece_x, piece_y + 1] == null;
                            }
                        }
                        else // standard Move
                        {
                            acceptable = final_y == piece_y + 1;
                        }
                    }
                }
                else
                {
                    acceptable = (final_x == piece_x - 1 || final_x == piece_x + 1) && final_y == piece_y + 1;
                }

            }


            return acceptable;
        }
    }
    class Knight : Piece
    {
        public Knight(string colour, double value) : base(colour)
        {
            this.colour = colour;
            code = "k";
            this.value = value;
            name = "Knight";
        }

        public override bool Piece_Check_Move(int piece_x, int piece_y, int final_x, int final_y, Piece[,] board)
        {
            bool acceptable = false;

            acceptable = (Math.Abs(final_x - piece_x) == 2 && Math.Abs(final_y - piece_y) == 1) || (Math.Abs(final_x - piece_x) == 1 && Math.Abs(final_y - piece_y) == 2);

            return acceptable;
        }
    }
    class Bishop : Piece
    {
        public Bishop(string colour, double value) : base(colour)
        {
            this.colour = colour;
            code = "b";
            this.value = value;
            name = "Bishop";
        }

        public override bool Piece_Check_Move(int piece_x, int piece_y, int final_x, int final_y, Piece[,] board)
        {
            bool acceptable = false;

            acceptable = Math.Abs(final_x - piece_x) == Math.Abs(final_y - piece_y);

            if (acceptable && Math.Abs(final_x - piece_x) > 1)
            {
                for (int i = 1; i < Math.Abs(final_x - piece_x); i++)
                {
                    if (board[piece_x + (i * ((final_x - piece_x) / (Math.Abs(final_x - piece_x)))), piece_y + (i * ((final_y - piece_y) / (Math.Abs(final_y - piece_y))))] != null)
                    {
                        acceptable = false;
                    }
                }
            }

            return acceptable;
        }
    }
    class King : Piece
    {
        public King(string colour, double value) : base(colour)
        {
            this.colour = colour;
            code = "K";
            this.value = value;
            name = "King";
        }

        public override bool Piece_Check_Move(int piece_x, int piece_y, int final_x, int final_y, Piece[,] board)
        {
            bool acceptable;

            acceptable = ((Math.Abs(final_x - piece_x) == Math.Abs(final_y - piece_y)) && Math.Abs(final_x - piece_x) == 1) || ((piece_x == final_x || piece_y == final_y) && ((Math.Abs(final_x - piece_x) == 1) || (Math.Abs(final_y - piece_y) == 1)));

            return acceptable;
        }
    }
    class Queen : Piece
    {
        public Queen(string colour, double value) : base(colour)
        {
            this.colour = colour;
            code = "Q";
            this.value = value;
            name = "Queen";
        }

        public override bool Piece_Check_Move(int piece_x, int piece_y, int final_x, int final_y, Piece[,] board)
        {
            bool acceptable = false;

            //castle like Movement

            acceptable = piece_x == final_x || piece_y == final_y;

            if (acceptable)
            {
                if (piece_x == final_x)
                {
                    if (piece_y < final_y)//DOWN
                    {
                        for (int i = piece_y + 1; i < final_y; i++)
                        {
                            if (board[piece_x, i] != null)
                            {
                                acceptable = false;
                            }
                        }
                    }
                    else//UP
                    {
                        for (int i = piece_y - 1; i > final_y; i--)
                        {
                            if (board[piece_x, i] != null)
                            {
                                acceptable = false;
                            }
                        }
                    }
                }
                else
                {
                    if (piece_x < final_x)//RIGHT 
                    {
                        for (int i = piece_x + 1; i < final_x; i++)
                        {
                            if (board[i, piece_y] != null)
                            {
                                acceptable = false;
                            }
                        }
                    }
                    else//LEFT
                    {
                        for (int i = piece_x - 1; i > final_x; i--)
                        {
                            if (board[i, piece_y] != null)
                            {
                                acceptable = false;
                            }
                        }
                    }
                }
            }
            else
            {
                acceptable = Math.Abs(final_x - piece_x) == Math.Abs(final_y - piece_y);

                if (acceptable)
                {
                    for (int i = 1; i < Math.Abs(final_x - piece_x); i++)
                    {
                        if (board[piece_x + (i * ((final_x - piece_x) / (Math.Abs(final_x - piece_x)))), piece_y + (i * ((final_y - piece_y) / (Math.Abs(final_y - piece_y))))] != null)
                        {
                            acceptable = false;
                        }
                    }
                }
            }


            return acceptable;

        }
    }
    class Board
    {
        public Piece[,] board_pieces = new Piece[8, 8];
        private string[,] board_shading = new string[8, 8];

        private const int dimensions = 8;
        public double board_value = 0;

        public bool is_CheckMate = false;

        public int comp_piece_x;
        public int comp_piece_y;
        public int comp_final_x;
        public int comp_final_y;

        public double[] piece_values = new double[6];

        public Board(double[] piece_values)
        {
            this.piece_values = piece_values;
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    board_pieces[i, j] = null;
                }
            }

            setup_board();
        }

        public void setup_board()
        {
            for (int i = 0; i < dimensions; i++)
            {
                board_pieces[i, 1] = new Pawn("B", piece_values[0]);
                board_pieces[i, 6] = new Pawn("W", piece_values[0]);
            }

            board_pieces[0, 0] = new Castle("B", piece_values[1]);
            board_pieces[7, 0] = new Castle("B", piece_values[1]);

            board_pieces[0, 7] = new Castle("W", piece_values[1]);
            board_pieces[7, 7] = new Castle("W", piece_values[1]);

            board_pieces[1, 0] = new Knight("B", piece_values[2]);
            board_pieces[6, 0] = new Knight("B", piece_values[2]);

            board_pieces[1, 7] = new Knight("W", piece_values[2]);
            board_pieces[6, 7] = new Knight("W", piece_values[2]);

            board_pieces[2, 0] = new Bishop("B", piece_values[3]);
            board_pieces[5, 0] = new Bishop("B", piece_values[3]);

            board_pieces[2, 7] = new Bishop("W", piece_values[3]);
            board_pieces[5, 7] = new Bishop("W", piece_values[3]);

            board_pieces[3, 0] = new Queen("B", piece_values[4]);
            board_pieces[3, 7] = new Queen("W", piece_values[4]);

            board_pieces[4, 0] = new King("B", piece_values[5]);
            board_pieces[4, 7] = new King("W", piece_values[5]);
        }

        private void show_board()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" ABCDEFGH");
            for (int i = 0; i < dimensions; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(8 - i);
                for (int j = 0; j < dimensions; j++)
                {
                    if (i % 2 != j % 2)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }

                    if (board_pieces[j, i] != null)
                    {
                        board_pieces[j, i].Format_Piece();
                        Console.Write(board_pieces[j, i].GetCode());
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public void Board_Refresh()
        {
            show_board();

            Console.ForegroundColor = ConsoleColor.Green;

            if (is_Check("W", board_pieces))
            {
                Console.WriteLine("White is in check");

                if (!any_possible_moves("W"))
                {
                    Console.WriteLine("Check Mate on White");
                    is_CheckMate = true;
                }
            }
            if (is_Check("B", board_pieces))
            {
                Console.WriteLine("Black is in check");
                if (!any_possible_moves("B"))
                {
                    Console.WriteLine("Check Mate on Black");
                    is_CheckMate = true;
                }
            }
        }

        public double calculate_board_value()
        {
            double total = 0;

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    if (board_pieces[j, i] != null)
                    {
                        if (board_pieces[j, i].colour == "W")
                        {
                            total += board_pieces[j, i].value;
                        }
                        else
                        {
                            total -= board_pieces[j, i].value;
                        }
                    }
                }
            }

            return total;
        }

        private bool Board_Check_Move(int piece_x, int piece_y, int final_x, int final_y, string turn)
        {
            bool acceptable = false;

            if (board_pieces[piece_x, piece_y] != null && board_pieces[piece_x, piece_y].colour == turn)
            {
                if (board_pieces[final_x, final_y] == null || board_pieces[final_x, final_y].colour != board_pieces[piece_x, piece_y].colour)
                {
                    acceptable = true;
                }
            }

            return acceptable;
        }

        public bool Check_Move(int piece_x, int piece_y, int final_x, int final_y, string turn)
        {
            Piece[,] copy = (Piece[,])board_pieces.Clone();
            Piece hold;

            bool acceptable = false;

            if (Board_Check_Move(piece_x, piece_y, final_x, final_y, turn))
            {
                if (board_pieces[piece_x, piece_y].Piece_Check_Move(piece_x, piece_y, final_x, final_y, board_pieces))
                {
                    acceptable = true;

                    hold = copy[piece_x, piece_y];
                    copy[piece_x, piece_y] = null;
                    copy[final_x, final_y] = hold;

                    if (copy[final_x, final_y].code == "p")
                    {
                        switch (final_y)
                        {
                            case 0:
                                copy[final_x, final_y] = new Queen(copy[final_x, final_y].colour, piece_values[4]);
                                break;
                            case 7:
                                copy[final_x, final_y] = new Queen(copy[final_x, final_y].colour, piece_values[4]);
                                break;
                            default:
                                break;
                        }
                    }

                    if (is_Check(turn, copy))
                    {
                        acceptable = false;
                    }
                }
            }


            return acceptable;
        }

        public bool any_possible_moves(string turn)
        {
            bool a_possible_move = false;

            for (int y = 0; y < dimensions; y++)
            {
                for (int x = 0; x < dimensions; x++)
                {

                    if (board_pieces[x, y] != null)
                    {
                        for (int i = 0; i < dimensions; i++)
                        {
                            for (int j = 0; j < dimensions; j++)
                            {
                                if (j != x || i != y)
                                {
                                    if (Check_Move(x, y, j, i, turn))
                                    {
                                        a_possible_move = true;
                                    }
                                }
                            }
                        }
                    }


                }
            }

            return a_possible_move;
        }

        public void Move(int piece_x, int piece_y, int final_x, int final_y, ref string turn)
        {
            Piece hold;
            if (Check_Move(piece_x, piece_y, final_x, final_y, turn))
            {
                hold = board_pieces[piece_x, piece_y];
                board_pieces[piece_x, piece_y] = null;
                board_pieces[final_x, final_y] = hold;

                if (board_pieces[final_x, final_y].code == "p")
                {
                    switch (final_y)
                    {
                        case 0:
                            board_pieces[final_x, final_y] = new Queen(board_pieces[final_x, final_y].colour, piece_values[4]);
                            break;
                        case 7:
                            board_pieces[final_x, final_y] = new Queen(board_pieces[final_x, final_y].colour, piece_values[4]);
                            break;
                        default:
                            break;
                    }
                }

                switch (turn)
                {
                    case "W":
                        turn = "B";
                        break;
                    case "B":
                        turn = "W";
                        break;
                    default:
                        break;
                }

                board_value = calculate_board_value();
            }
        }

        public void Move_input(string start, string end, ref string turn)
        {
            int piece_x;
            int piece_y;
            int final_x;
            int final_y;

            if(validate(start) && validate(end))
            {
                piece_x = extract_x(start);
                piece_y = extract_y(start);
                final_x = extract_x(end);
                final_y = extract_y(end);

                if (validate_ex(piece_x) && validate_ex(piece_y) && validate_ex(final_x) && validate_ex(final_y))
                {
                    Move(piece_x, piece_y, final_x, final_y, ref turn);
                }
            }
        }

        private bool validate(string input)
        {
            bool acceptable = false;

            if(input.Length == 2)
            {
                if(int.TryParse(input.Substring(1), out int int_1))
                {
                    if (char.IsLetter(input[0]))
                    {
                        acceptable = true;
                    }    
                }
            }

            return acceptable;
        }

        private int extract_x(string input)
        {
            int x;
            char letter = char.Parse(input.Substring(0, 1));
            x = (int)letter - 65;
            return x;
        }
        private int extract_y(string input)
        {
            int y;
            y = int.Parse(input.Substring(1));
            y = 8 - y;
            return y;
        }

        private bool validate_ex(int input)
        {
            return input >= 0 && input < 8;
        }

        public bool is_Check(string colour, Piece[,] board)
        {
            int king_x = 0;
            int king_y = 0;

            bool Check = false;

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    if (board[j, i] != null)
                    {
                        if (board[j, i].colour == colour && board[j, i].code == "K")
                        {
                            king_x = j;
                            king_y = i;
                        }
                    }
                }
            }

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    if (board[j, i] != null && board[j, i].colour != colour)
                    {
                        if (board[j, i].Piece_Check_Move(j, i, king_x, king_y, board))
                        {
                            Check = true;
                        }
                    }
                }
            }

            return Check;
        }

        public void state_move(int piece_x, int piece_y, int final_x, int final_y, string turn)
        {
            string piece = "";
            string destination = "";

            piece = board_pieces[piece_x, piece_y].name;

            switch (final_x)
            {
                case 0:
                    destination = "A" + (8 - final_y).ToString();
                    break;
                case 1:
                    destination = "B" + (8 - final_y).ToString();
                    break;
                case 2:
                    destination = "C" + (8 - final_y).ToString();
                    break;
                case 3:
                    destination = "D" + (8 - final_y).ToString();
                    break;
                case 4:
                    destination = "E" + (8 - final_y).ToString();
                    break;
                case 5:
                    destination = "F" + (8 - final_y).ToString();
                    break;
                case 6:
                    destination = "G" + (8 - final_y).ToString();
                    break;
                case 7:
                    destination = "H" + (8 - final_y).ToString();
                    break;
                default:
                    break;
            }

            Console.WriteLine($"{turn} : {piece} to {destination}");
        }

    }
    class Computer
    {
        public int move_piece_x = 0;
        public int move_piece_y = 1;
        public int move_final_x = 0;
        public int move_final_y = 2;

        public int piece_x = 0;
        public int piece_y = 0;
        public int final_x = 0;
        public int final_y = 0;

        public double lowest_board = 10000;

        public string colour;

        List<Board> board_1 = new List<Board>();
        List<Board> board_2 = new List<Board>();
        List<Board> board_3 = new List<Board>();
        List<Board> board_4 = new List<Board>();
        List<Board> board_5 = new List<Board>();
        List<Board> board_6 = new List<Board>();

        Board best_board;
        Board best_board_2;
        Board best_board_4;
        Board best_board_6;

        double[] piece_values = new double[6];

        public Computer(string colour, double[] piece_values)
        {
            this.colour = colour;
            this.piece_values = piece_values;
        }

        public List<Board> gen_boards(Board initial_board, string turn)
        {
            List<Board> boards = new List<Board>();
            Piece hold;
            int index = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (initial_board.board_pieces[j, i] != null && initial_board.board_pieces[j, i].colour == turn)
                    {
                        for (int n = 0; n < 8; n++)
                        {
                            for (int m = 0; m < 8; m++)
                            {
                                if (!(m == i && n == j))
                                {
                                    if (initial_board.Check_Move(j, i, m, n, turn))
                                    {
                                        piece_x = j;
                                        piece_y = i;
                                        final_x = m;
                                        final_y = n;

                                        boards.Add(new Board(piece_values));
                                        boards[index].board_pieces = (Piece[,])initial_board.board_pieces.Clone();
                                        hold = boards[index].board_pieces[piece_x, piece_y];
                                        boards[index].board_pieces[piece_x, piece_y] = null;
                                        boards[index].board_pieces[final_x, final_y] = hold;

                                        if (boards[index].board_pieces[final_x, final_y].code == "p")
                                        {
                                            switch (n)
                                            {
                                                case 0:
                                                    boards[index].board_pieces[final_x, final_y] = new Queen(boards[index].board_pieces[final_x, final_y].colour, piece_values[4]);
                                                    break;
                                                case 7:
                                                    boards[index].board_pieces[final_x, final_y] = new Queen(boards[index].board_pieces[final_x, final_y].colour, piece_values[4]);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        boards[index].comp_piece_x = piece_x;
                                        boards[index].comp_piece_y = piece_y;
                                        boards[index].comp_final_x = final_x;
                                        boards[index].comp_final_y = final_y;

                                        index++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return boards;
        }

        public Board Best(List<Board> boards, string turn)
        {
            double best_value = 0;
            Board best_board_2 = boards[0];

            switch (turn)
            {
                case "W":
                    best_value = -1000;
                    break;
                case "B":
                    best_value = 1000;
                    break;
                default:
                    break;
            }

            foreach (Board board in boards)
            {
                switch (turn)
                {
                    case "W":
                        if (board.calculate_board_value() > best_value)
                        {
                            best_board_2 = board;
                            best_value = board.calculate_board_value();
                        }
                        break;
                    case "B":
                        if (board.calculate_board_value() < best_value)
                        {
                            best_board_2 = board;
                            best_value = board.calculate_board_value();
                        }
                        break;
                    default:
                        best_board_2 = boards[0];
                        break;
                }
            }

            return best_board_2;
        }

        public void move(Board initial_board)
        {
            switch (colour)
            {
                case "B":
                    lowest_board = 1000;
                    break;
                case "W":
                    lowest_board = -1000;
                    break;
            }

            board_1 = gen_boards(initial_board, colour);

            foreach (Board board in board_1)
            {
                board_2 = gen_boards(board, S(colour));
                if (board_2.Count == 0)
                {
                    if (!board.any_possible_moves(S(colour)))
                    {
                        switch (colour)
                        {
                            case "B":
                                if (lowest_board > -900)
                                {
                                    best_board = board;
                                    lowest_board = -900;
                                }
                                break;
                            case "W":
                                if (lowest_board < 900)
                                {
                                    best_board = board;
                                    lowest_board = 900;
                                }
                                break;
                        }

                    }
                }
                else
                {
                    best_board_2 = Best(board_2, S(colour));

                    board_3 = gen_boards(best_board_2, colour);
                    if (board_3.Count == 0)
                    {
                    }
                    else
                    {
                        foreach (Board board_b in board_3)
                        {
                            board_4 = gen_boards(board_b, S(colour));
                            if (board_4.Count == 0)
                            {
                                if (!board_b.any_possible_moves(S(colour)))
                                {
                                    switch (colour)
                                    {
                                        case "B":
                                            if (lowest_board > -800)
                                            {
                                                best_board = board;
                                                lowest_board = -800;
                                            }
                                            break;
                                        case "W":
                                            if (lowest_board < 800)
                                            {
                                                best_board = board;
                                                lowest_board = 800;
                                            }
                                            break;
                                    }

                                }

                            }
                            else
                            {
                                best_board_4 = Best(board_4, S(colour));

                                board_5 = gen_boards(best_board_4, colour);
                                if (board_5.Count == 0)
                                {
                                }
                                else
                                {
                                    foreach (Board board_c in board_5)
                                    {
                                        board_6 = gen_boards(board_c, S(colour));
                                        if (board_6.Count == 0)
                                        {
                                            if (!board_c.any_possible_moves(S(colour)))
                                            {
                                                switch (colour)
                                                {
                                                    case "B":
                                                        if (lowest_board > -700)
                                                        {
                                                            best_board = board;
                                                            lowest_board = -700;
                                                        }
                                                        break;
                                                    case "W":
                                                        if (lowest_board < 700)
                                                        {
                                                            best_board = board;
                                                            lowest_board = 700;
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            best_board_6 = Best(board_6, S(colour));

                                            switch (colour)
                                            {
                                                case "B":
                                                    if (board.calculate_board_value() + best_board_2.calculate_board_value() + board_b.calculate_board_value() + best_board_4.calculate_board_value() + board_c.calculate_board_value() + best_board_6.calculate_board_value() < lowest_board)
                                                    {
                                                        best_board = board;
                                                        lowest_board = board.calculate_board_value() + best_board_2.calculate_board_value() + board_b.calculate_board_value() + best_board_4.calculate_board_value();
                                                    }
                                                    break;
                                                case "W":
                                                    if (board.calculate_board_value() + best_board_2.calculate_board_value() + board_b.calculate_board_value() + best_board_4.calculate_board_value() + board_c.calculate_board_value() + best_board_6.calculate_board_value() > lowest_board)
                                                    {
                                                        best_board = board;
                                                        lowest_board = board.calculate_board_value() + best_board_2.calculate_board_value() + board_b.calculate_board_value() + best_board_4.calculate_board_value();
                                                    }
                                                    break;
                                            }
                                            

                                        }
                                    }
                                }

                            }
                        }
                    }
                }

            }


            move_piece_x = best_board.comp_piece_x;
            move_piece_y = best_board.comp_piece_y;

            move_final_x = best_board.comp_final_x;
            move_final_y = best_board.comp_final_y;
        }

        public string S(string input)
        {
            string alt = "";
            switch (input)
            {
                case "W":
                    alt = "B";
                    break;
                case "B":
                    alt = "W";
                    break;
            }

            return alt;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            string choice = "";

            int unique_game_code = 0;
            string game_code_read = "";
            string unique_game_code_input = "";

            string turn = "W";

            string game_load_temp;

            string start = "";
            string end = "";

            do
            {
                Console.WriteLine("1) Start a new Game");
                Console.WriteLine("2) Load an old Game");
                Console.WriteLine("3) Watch the computer play against itself");
                Console.Write("What do you want to do: ");
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2" && choice != "3");

            double[] piece_values = new double[6] { 1, 5, 3, 3, 9, 1000 }; // pawn,castle,knight,bishop,queen,king

            bool human_is_playing = choice != "3";
            
            string computer_player = "";

            if (choice == "1")
            {
                do
                {
                    Console.Write("Choose the computer's colour (W/B): ");
                    computer_player = Console.ReadLine();
                } while (!(computer_player == "W" || computer_player == "B"));

                StreamReader sr = new StreamReader("UniqueCode.txt");
                game_code_read = sr.ReadLine();
                if(game_code_read != null)
                {
                    unique_game_code = int.Parse(game_code_read);
                }
                else
                {
                    unique_game_code = 0;
                }
                sr.Close();
                    
                StreamWriter sw = new StreamWriter("UniqueCode.txt");
                sw.WriteLine(unique_game_code + 1);
                sw.Close();

                Console.WriteLine("Your unique game code is: " + unique_game_code);
                Console.ReadLine();

            }
            else
            {
                computer_player = "W";
            }

            Board board = new Board(piece_values);

            if(choice == "2")
            {
                Console.Write("Enter the unique game code: ");
                unique_game_code_input = Console.ReadLine();
                unique_game_code = int.Parse(unique_game_code_input);

                StreamReader sr = new StreamReader(unique_game_code_input + ".txt");

                while (!sr.EndOfStream)
                {
                    game_load_temp = sr.ReadLine();

                    if(game_load_temp.Substring(0, 1) == "Z")
                    {
                        computer_player = game_load_temp.Substring(0, 1);
                        turn = game_load_temp.Substring(1);
                    }
                    else if(game_load_temp.Length == 2)
                    {
                        board.board_pieces[int.Parse(game_load_temp.Substring(0, 1)), int.Parse(game_load_temp.Substring(1, 1))] = null;
                    }
                    else
                    {
                        switch (game_load_temp.Substring(3))
                        {
                            case "p":
                                board.board_pieces[int.Parse(game_load_temp.Substring(0, 1)), int.Parse(game_load_temp.Substring(1, 1))] = new Pawn(game_load_temp.Substring(2, 1), board.piece_values[0]);
                                break;
                            case "c":
                                board.board_pieces[int.Parse(game_load_temp.Substring(0, 1)), int.Parse(game_load_temp.Substring(1, 1))] = new Castle(game_load_temp.Substring(2, 1), board.piece_values[1]);
                                break;
                            case "k":
                                board.board_pieces[int.Parse(game_load_temp.Substring(0, 1)), int.Parse(game_load_temp.Substring(1, 1))] = new Knight(game_load_temp.Substring(2, 1), board.piece_values[2]);
                                break;
                            case "b":
                                board.board_pieces[int.Parse(game_load_temp.Substring(0, 1)), int.Parse(game_load_temp.Substring(1, 1))] = new Bishop(game_load_temp.Substring(2, 1), board.piece_values[3]);
                                break;
                            case "Q":
                                board.board_pieces[int.Parse(game_load_temp.Substring(0, 1)), int.Parse(game_load_temp.Substring(1, 1))] = new Queen(game_load_temp.Substring(2, 1), board.piece_values[4]);
                                break;
                            case "K":
                                board.board_pieces[int.Parse(game_load_temp.Substring(0, 1)), int.Parse(game_load_temp.Substring(1, 1))] = new King(game_load_temp.Substring(2, 1), board.piece_values[5]);
                                break;
                        }
                    }
                }
                sr.Close();
            }

            Computer comp = new Computer(computer_player, piece_values);
            Computer comp_2 = new Computer("B", piece_values);

            while (!board.is_CheckMate)
            {
                Console.Clear();
                Console.WriteLine(unique_game_code);
                board.Board_Refresh();
                if (!board.is_CheckMate)
                {
                    go(turn);

                    if (human_is_playing)
                    {
                        if (turn == computer_player)
                        {
                            comp.move(board);
                            board.Move(comp.move_piece_x, comp.move_piece_y, comp.move_final_x, comp.move_final_y, ref turn);
                            board.state_move(comp.move_final_x, comp.move_final_y, comp.move_final_x, comp.move_final_y, computer_player);
                            Thread.Sleep(2500);
                        }
                        else
                        {
                            Console.Write("From: ");
                            start = Console.ReadLine();
                            Console.Write("To: ");
                            end = Console.ReadLine();
                            board.Move_input(start, end, ref turn);
                        }

                        StreamWriter sw = new StreamWriter(unique_game_code + ".txt");
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if(board.board_pieces[j, i] != null)
                                {
                                    sw.WriteLine(j + "" + i + "" + board.board_pieces[j, i].colour + board.board_pieces[j, i].code);
                                }
                                else
                                {
                                    sw.WriteLine(j + "" + i + "");
                                }
                            }
                        }
                        sw.WriteLine("Z" + computer_player + turn);
                        sw.Close();
                    }
                    else
                    {
                        if (turn == "W")
                        {
                            comp.move(board);
                            board.Move(comp.move_piece_x, comp.move_piece_y, comp.move_final_x, comp.move_final_y, ref turn);
                        }
                        else
                        {
                            comp_2.move(board);
                            board.Move(comp_2.move_piece_x, comp_2.move_piece_y, comp_2.move_final_x, comp_2.move_final_y, ref turn);
                        }
                    }    
                }           
            }
            
            if (turn != computer_player)
            {
                board.state_move(comp.move_final_x, comp.move_final_y, comp.move_final_x, comp.move_final_y, computer_player);
            }

            Console.WriteLine("Good Game");
        }

        static void go(string input)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            switch (input)
            {
                case "W":
                    Console.WriteLine("White's turn");
                    break;
                case "B":
                    Console.WriteLine("Black's turn");
                    break;
                default:
                    break;
            }
        }
    }
}
