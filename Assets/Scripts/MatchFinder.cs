﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<Gem> currentMatches = new List<Gem>();
    private void Awake()
    {
        board = FindObjectOfType<Board>();

    }
    public void FinalAllMatches()
    {
        currentMatches.Clear();

        for(int x = 0; x < board.width; x++)
        {
            for(int y = 0; y < board.height; y++)
            {
                Gem currenGem = board.allGems[x, y];
                if(currenGem != null)
                {
                    if(x>0 && x <board.width - 1)
                    {
                        Gem leftGem = board.allGems[x - 1, y];
                        Gem rightGem = board.allGems[x + 1, y];
                        if (leftGem != null && rightGem != null)
                        {
                            if(leftGem.type==currenGem.type&& rightGem.type == currenGem.type)
                            {
                                currenGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                currentMatches.Add(currenGem);
                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);

                            }
                        }
                    }


                    if (y > 0 && y < board.height - 1)
                    {
                        Gem aboveGem = board.allGems[x , y+ 1];
                        Gem belowGem = board.allGems[x , y- 1];
                        if (aboveGem != null && belowGem != null)
                        {
                            if (aboveGem.type == currenGem.type && belowGem.type == currenGem.type)
                            {
                                currenGem.isMatched = true;
                                aboveGem.isMatched = true;
                                belowGem.isMatched = true;

                                currentMatches.Add(currenGem);
                                currentMatches.Add(aboveGem);
                                currentMatches.Add(belowGem);
                            }
                        }
                    }
                }
            }
        }
        if (currentMatches.Count > 0)
        { 
            currentMatches = currentMatches.Distinct().ToList();
        }

        CheckForBombs();
    }
    public void CheckForBombs()
    {
        for(int i = 0; i < currentMatches.Count; i++)
        {
            Gem gem = currentMatches[i];

            int x = gem.posIndex.x;
            int y = gem.posIndex.y;

            if(gem.posIndex.x > 0)
            {
                if (board.allGems[x-1,y] != null)
                {
                    if (board.allGems[x - 1, y].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int (x-1,y),board.allGems[x-1,y]);
                    }
                }
            }


            if (gem.posIndex.x < board.width -1)
            {
                if (board.allGems[x + 1, y] != null)
                {
                    if (board.allGems[x + 1, y].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y), board.allGems[x + 1, y]);
                    }
                }
            }


            if (gem.posIndex.y > 0)
            {
                if (board.allGems[x , y - 1] != null)
                {
                    if (board.allGems[x, y - 1].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x , y - 1), board.allGems[x, y - 1]);
                    }
                }
            }


            if (gem.posIndex.y < board.height -1 )
            {
                if (board.allGems[x, y + 1] != null)
                {
                    if (board.allGems[x, y + 1].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y + 1), board.allGems[x, y + 1]);
                    }
                }
            }
        }
    }

    public void MarkBombArea(Vector2Int bombPos,Gem theBomb)
    {
        for(int x= bombPos.x - theBomb.blastSize; x <= bombPos.x + theBomb.blastSize; x++)
        {
            for(int y=bombPos.y - theBomb.blastSize; y <=bombPos.y + theBomb.blastSize; y++)
            {
                if(x >=0 && x < board.width && y >=0 && y < board.height)
                {
                    if (board.allGems[x, y] != null)
                    {
                        board.allGems[x, y].isMatched = true;
                        currentMatches.Add(board.allGems[x, y]);
                    }
                    
                }
            }
        }

        currentMatches = currentMatches.Distinct().ToList();
    }
}
