using BlindChess.Tournaments;
using BlindChessEngine;
using BlindChessStrategies.Default;

Dictionary<string, (IGameStrategy, IBlindStrategy)> players = new();
players["Default1"] = (new DefaultGameStrategy(), new DefaultBlindStrategy());
players["Default2"] = (new DefaultGameStrategy(), new DefaultBlindStrategy());
var tournament = new PairTournament(players, 40);
tournament.Play();
tournament.PrintResults();
