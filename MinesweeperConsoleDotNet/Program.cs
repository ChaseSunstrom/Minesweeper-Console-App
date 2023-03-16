using System.Reflection;

int dimensions = 0;
Random rnd = new Random();
Boolean gameLoop = true;
int turn = 0;
int updateLoopCounter = 0;
bool hideValueCalled = false;
int minesAmount = 0;

Console.WriteLine("Welcome to Minesweeper!");
Console.WriteLine("Every time you select an empty space, the value will show up as a 'E' and if you hit a mine the game will end. Good luck!\n");
Console.WriteLine("What board size do you want to play with? \nSmall \nMedium \nLarge");
string boardSize = Console.ReadLine().ToLower();
Console.Clear();

//Checks the user selected value to see if they chose a small board size
if (boardSize.Contains("small"))
{
    //Changes the dimensions variable to 10
    dimensions = 10;
    CreateRandomBoard(dimensions);
}
//Checks the user selected value to see if they chose a medium board size
else if (boardSize.Contains("medium"))
{
    //Changes the dimensions variable to 15
    dimensions = 15;
    CreateRandomBoard(dimensions);
}
//Checks the user selected value to see if they chose a large board size
else if (boardSize.Contains("large"))
{
    //Changes the dimensions variable to 20
    dimensions = 20;
    CreateRandomBoard(dimensions);
}

//Checks the board to see if the user selected space was a 1
bool CheckBoard(int _width, int _height, string[,] _board, string[,] _original)
{
    for (int i = 0; i < _board.GetLength(0); i++)
    {
        for (int k = 0; k < _board.GetLength(1); k++)
        {
            if (_original[i, k] == "1" && _original[_width, _height] == "1")
            {
                return false;
            }
        }
    }

    return true;
}
//Generates a game board from the 2d array of random numbers
string[,] GenerateBoard(string[,] _randomBoard)
{
    _randomBoard.Clone();

    for (int i = 0; i < dimensions; i++)
    {
        for (int j = 0; j < dimensions; j++)
        {
            if (_randomBoard[i, j].Contains('1') || _randomBoard[i, j].Contains('2'))
            {
                _randomBoard[i, j] = "1";
            }

            else
            {
                _randomBoard[i, j] = "0";
            }
        }
    }

    return _randomBoard;
}
//changes the board into an empty board so the user can not see where the mines are
string[,] HideValues(string[,] _board)
{
    for (int i = 0; i < _board.GetLength(0); i++)
    {
        for (int k = 0; k < _board.GetLength(1); k++)
        {
            _board[i, k] = " ";
        }
    }
    hideValueCalled= true;
    return _board;
}
//prints the current game board 
void PrintBoard(string[,] _board)
{
    string boardBottom = new string('-', dimensions*4-1);

    Console.Write("    ");

    for (int i = 0; i < _board.GetLength(1); i++)
    {
        if (i <= 9)
        {
            Console.Write(" " + i + "  ");
        }

        else
        {
            Console.Write(i + "  ");
        }
    }

    Console.Write("\n    " + boardBottom + "\n");

    for (int i = 0; i < _board.GetLength(0); i++)
    {
        if (i <= 9)
        {
            Console.Write(i + "   ");
        }

        else
        {
            Console.Write(i + "  ");
        }

        for (int k = 0; k < _board.GetLength(1); k++)
        {
            if (_board[i, k] == "0")
            {
                _board[i, k] = " ";
            }

            Console.Write("|" + _board[i, k] + "| ");
        }
        //next row
        Console.Write("\n    " + boardBottom + "\n");
    }
}
//updates the current game board so the user can see if the value they selected was a mine or not
string[,] UpdateBoard(int _width, int _height, string[,] _board, string[,] _originalBoard)
{
    turn++;

    if (_board[_width, _height].Contains('F'))
    {
        // Remove the flag
        _board[_width, _height] = " ";
    }
    else
    {
        // Set the flag
        _board[_width, _height] = "F";
    }

    if (_originalBoard[_width, _height] == "1" && !_board[_width, _height].Contains('F'))
    {
        // The selected position contains a mine, end the game
        Console.WriteLine("Game over!");
        gameLoop = false;
    }
    else if (_originalBoard[_width, _height] == "0")
    {
        // Update the current position with an "E" to indicate an empty space
        _board[_width, _height] = "E";

        int adjacentMines = 0;

        // Loop up to 10 times or until no updates were made
        for (int i = 0; i < 30; i++)
        {
            // Keep track of whether any updates were made during this iteration
            bool updated = false;

            // Check all adjacent spaces
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int newX = _width + x;
                    int newY = _height + y;

                    // Check that the adjacent space is within bounds and not the current space
                    if (newX >= 0 && newX < _originalBoard.GetLength(0) && newY >= 0 && newY < _originalBoard.GetLength(1) && !(x == 0 && y == 0))
                    {
                        // Check if the adjacent space contains a mine
                        if (_originalBoard[newX, newY] == "1")
                        {
                            adjacentMines++;
                        }
                    }
                }
            }

            // Update the current space with the mine count or "E" if no mines adjacent
            if (_board[_width, _height] != "E")
            {
                if (adjacentMines > 0)
                {
                    _board[_width, _height] = adjacentMines.ToString();
                }
                else
                {
                    _board[_width, _height] = "E";
                }
            }

            // Check all adjacent spaces again to update them if necessary
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int newX = _width + x;
                    int newY = _height + y;

                    // Check that the adjacent space is within bounds and not the current space
                    if (newX >= 0 && newX < _originalBoard.GetLength(0) && newY >= 0 && newY < _originalBoard.GetLength(1) && !(x == 0 && y == 0))
                    {
                        // Check if the adjacent space is empty and not already updated
                        if (_originalBoard[newX, newY] == "0" && _board[newX, newY] != "E")
                        {
                            // Check if the adjacent space contains any mines
                            int adjMines = 0;
                            for (int a = -1; a <= 1; a++)
                            {
                                for (int b = -1; b <= 1; b++)
                                {
                                    int adjX = newX + a;
                                    int adjY = newY + b;

                                    // Check that the adjacent space is within bounds and not the current space
                                    if (adjX >= 0 && adjX < _originalBoard.GetLength(0) && adjY >= 0 && adjY < _originalBoard.GetLength(1) && !(a == 0 && b == 0))
                                    {
                                        if (_originalBoard[adjX, adjY] == "1")
                                        {
                                            adjMines++;
                                        }
                                    }
                                }

                                // Update the adjacent space with the mine count or "E" if no mines adjacent
                                if (adjMines > 0)
                                {
                                    _board[newX, newY] = adjMines.ToString();
                                }
                                else
                                {
                                    _board[newX, newY] = "E";
                                }

                                // Set the updated flag to true
                                updated = true;
                            }
                        }
                    }
                }
            }

            // If no updates were made during this iteration, break out of the loop
            if (!updated)
            {
                break;
            }
        }
    }

    return _board;
}

//if it is the users first turn and the space the user selected was a 1 it will override that space and make it empty and will also call the UpdateBoard method to check if neighboring spaces are also empty
string[,] OverrideBoard(int _width, int _height, string[,] _board)
{
    // Check if it is the first turn of the game
    if (turn == 0)
    {
        // Update the selected cell to "E"
        _board[_width, _height] = "E";
    }
    return _board;
}

void CreateRandomBoard(int dimensions)
{
    string[,] board = new string[dimensions, dimensions];

    //Cycles over the entire 2d array and creates random numbers for each element
    for (int i = 0; i < dimensions; i++)
    {
        for (int j = 0; j < dimensions; j++)
        {
            board[i, j] = rnd.Next(100).ToString();
        }
    }
    string[,] clonedBoard = board.Clone() as string[,];
    Game(board, clonedBoard);
}

bool CheckGameEnd(string[,] _board, string[,] _originalBoard)
{
    for (int i = 0; i < _board.GetLength(0); i++)
    {
        for (int j = 0; j < _board.GetLength(1); j++)
        {
            if ((_board[i, j] == "E" || _board[i, j] == "1" || _board[i, j] == "2" ||
                _board[i, j] == "3" || _board[i, j] == "4" || _board[i, j] == "5" ||
                _board[i, j] == "6" || _board[i, j] == "7" || _board[i, j] == "8" ||
                _board[i, j] == "9") != _originalBoard[i, j].Contains('0'))
            {
                return false;
            }
            else if (_board[i, j] == "F" && _originalBoard[i, j] != "1")
            {
                return false;
            }
        }
    }

    return true;
}

bool CheckFalseFlags(string[,] _board, string[,] _originalBoard)
{
    for(int i = 0; i < _board.GetLength(0); i++)
    {
        for(int j = 0; j < _board.GetLength(1); j++)
        {
            if(!CheckGameEnd(_board, _originalBoard) && (_board[i, j].Contains('F') && _originalBoard[i, j].Contains('0')))
            {
                return false;
            }
        }
    }

    return true;
}

string[,] SetFlag(int height, int width, string[,] _board, string[,] _originalBoard)
{
    for(int i = 0; i < _board.GetLength(0); i++) 
    {
        for(int j = 0; j < _board.GetLength(1); j++) 
        {
            if (_originalBoard[i, j] == "1" && (_board[height, width] != "F" 
                || _board[height, width] != "E" || _board[height, width] != "1" 
                || _board[height, width] != "2" || _board[height, width] != "3" 
                || _board[height, width] != "4" || _board[height, width] != "5" 
                || _board[height, width] != "6" || _board[height, width] != "7" 
                || _board[height, width] != "8" || _board[height, width] != "9"))
            {
                _board[height, width] = "F";
            }
        }
    }

    return _board;
}

int CountMines(string[,] _board)
{
    for (int i = 0; i < _board.GetLength(0); i++)
    {
        for (int j = 0; j < _board.GetLength(1); j++)
        {
            if (_board[i, j] == "1")
            {
                minesAmount++;
            }   
        }
    }

    return minesAmount;
}
//creates a gameloop method which will be called with each board size 
void Game(string[,] board, string[,] clonedBoard)
{
    string[,] originalBoard = GenerateBoard(board);
    string[,] theBoard = GenerateBoard(clonedBoard);
    int userWidth;
    int userHeight;
    int flagWidth;
    int flagHeight;
    int minesFlagged = 0;

    PrintBoard(HideValues(theBoard));
    
    
    //asks the user for a width and height
    while (gameLoop && minesFlagged < CountMines(board))
    {
        try
        {
            if (turn >= 1)
            {
                Console.WriteLine("Would you like to set a flag? Y/N");
                string flagSet = Console.ReadLine();

                if (flagSet != null && flagSet.Contains('y'))
                {

                    Console.WriteLine("Enter a width value.");
                    flagWidth = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter a height value.");
                    flagHeight = Convert.ToInt32(Console.ReadLine());

                    theBoard = SetFlag(flagWidth, flagHeight, theBoard, originalBoard);
                    UpdateBoard(flagWidth, flagHeight, theBoard, originalBoard);
                }
            }

            Console.WriteLine("Enter a height value.");
            userWidth = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a width value.");
            userHeight = Convert.ToInt32(Console.ReadLine());

            //overrides board if it is the users first turn
            theBoard = OverrideBoard(userWidth, userHeight, theBoard);
            //updates the board every game loop with the user entered values
            theBoard = UpdateBoard(userWidth, userHeight, theBoard, originalBoard);
            //creates another boolean from checkboard that is different from game loop
            bool tempLoop = CheckBoard(userHeight, userWidth, theBoard, originalBoard);

            
            

            //uses temploop to determine if the user hit a mine and tells the user that they lost and displays the original board
            if (!tempLoop)
            {
                Console.WriteLine("You hit a mine: \n");
                PrintBoard(originalBoard);
            }

            else
            {
                if (CheckGameEnd(theBoard, originalBoard) && CheckFalseFlags(theBoard, originalBoard))
                {
                    Console.WriteLine("You won! Final game board:");
                    PrintBoard(theBoard);
                    gameLoop= false;
                }

                PrintBoard(theBoard);
                CheckGameEnd(theBoard, originalBoard);
            }
            //sets the gameloop to the other loop variable
            gameLoop = tempLoop;
            
        }
        //if the user did not enter a value when prompted it will throw an exception and tell them to enter a value
        catch (Exception e) 
        {
            Console.WriteLine("Enter a value please. \n");
            PrintBoard(theBoard);
        }
    }
}