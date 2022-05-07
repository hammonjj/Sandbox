import json
import urllib.parse
import urllib.request

import MatchData.team as t
import MatchData.champion as champion
import MatchData.summoner as pd
import MatchData.game_constants as gc
from riot_api_calls import get_summoner_mmr
from riot_api_calls import get_champion_mastery


class MatchData:
    def __init__(self, raw_data):
        json_data = json.loads(raw_data)

        self.teams = {}
        self.summoners = []

        # Single Values
        self.blueWin = False
        self.gameId = json_data["gameId"]
        self.gameType = json_data["gameType"]
        self.gameMode = json_data["gameMode"]
        self.platformId = json_data["platformId"]
        self.gameVersion = json_data["gameVersion"]

        self.mapId = json_data["mapId"]
        self.queueId = json_data["queueId"]
        self.seasonId = json_data["seasonId"]

        # Objects
        self.get_team_stats(json_data["teams"])
        self.get_players(json_data["participantIdentities"])
        self.get_picks(json_data["participants"])

        self.matchMmr = self.calculate_match_mmr()

    @staticmethod
    def validate_match(match):
        if not match["seasonId"] in gc.season:
            raise ValueError("Unknown seasonId: " + match["seasonId"])
        if not match["queueId"] in gc.queue_id:
            raise ValueError("Unknown queueId: " + match["queueId"])
        if not match["mapId"] in gc.map_id:
            raise ValueError("Unknown mapId: " + match["mapId"])

    def get_team_stats(self, teams):
        for team in teams:
            team_data = t.Team(team["teamId"])

            if team_data.teamId == 100:
                self.blueWin = team["win"] == "Win"

            team_data.bans = self.get_bans(team["bans"])
            self.teams[team_data.teamId] = team_data

    def get_players(self, players):
        for player in players:
            self.summoners.append(pd.Summoner(player))

    @staticmethod
    def get_bans(bans):
        champions = []
        for ban in bans:
            champions.append(ban["championId"])

        return champions

    def get_picks(self, participants):
        for participant in participants:
            champion_id = participant["championId"]

            # Query for Mastery
            el = [x for x in self.summoners if x.participantId == participant["participantId"]][0]
            mastery = urllib.request.urlopen(get_champion_mastery(el.summonerId, champion_id, self.platformId)).read()
            mastery = json.loads(mastery)

            champ = champion.Champion(
                champion_id,
                mastery["championPoints"],
                mastery["championLevel"],
                participant["timeline"]["lane"]
            )

            self.teams[participant["teamId"]].picks.append(champ)

    def calculate_match_mmr(self):
        total_mmr = 0
        for summoner in self.summoners:
            try:
                encoded_summoner_name = urllib.parse.quote_plus(summoner.summonerName)
                req = urllib.request.Request(
                    get_summoner_mmr(encoded_summoner_name),
                    headers = {
                        "User-Agent": "OSX:com.example.lolmatchpredictor:v1.0.0"
                    }
                )

                mmr = urllib.request.urlopen(req).read()
                mmr = json.loads(mmr)

                total_mmr += 1000 if mmr["ranked"]["avg"] is None else mmr["ranked"]["avg"]
            except Exception as ex:
                print("Exception Querying for MMR: " + ex)

        return total_mmr / 10
