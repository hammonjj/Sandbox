import unittest

# https://docs.python.org/3/library/unittest.html
from MatchData.match_data import MatchData


class MatchDataMethods(unittest.TestCase):
    def setUp(self):
        self.match_json = open("TestData/match_v3.json")

    def test_match_parsing(self):
        match_data = MatchData(self.match_json)
        self.assertEqual(match_data.seasonId, 11)
        self.assertEqual(match_data.gameId, 2956520234)
        self.assertEqual(match_data.queueId, "Summoner's Rift - Ranked")


if __name__ == '__main__':
    unittest.main()
