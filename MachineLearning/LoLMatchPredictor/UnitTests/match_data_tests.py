import json
from unittest import TestCase

from MatchData.match_data import MatchData


class MatchDataTests(TestCase):
    def setUp(self):
        file = open("TestData/match_v3.json");
        self.match_json = file.read()
        file.close()

    def test_match_parsing(self):
        match_data = MatchData(self.match_json)

        json_obj = json.dumps(match_data, default = lambda o: o.__dict__)
        print(json_obj)
        # self.assertEqual(match_data.seasonId, 11)
        # self.assertEqual(match_data.gameId, 2956520234)
        # self.assertEqual(match_data.queueId, 420)
