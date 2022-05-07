from enum import Enum

MATCH = "v4"
SUMMONER = "v4"
CHAMPION_MASTERY = "v4"

RIOT_API_KEY = "RGAPI-92b2746b-58e6-4055-adb3-1d08d0136667"


class Regions(Enum):
    def __str__(self):
        return str(self.value)

    RU = "ru"
    KR = "kr"
    BR1 = "br1"
    OC1 = "oc1"
    JP1 = "jp1"
    NA1 = "na1"
    EUN1 = "eun1"
    EUW1 = "euw1"
    TR1 = "tr1"
    LA1 = "la1"
    LA2 = "la2"


def get_summoner_info(summoner, region):
    return f"https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{summoner}?api_key={RIOT_API_KEY}"


def get_match_info(match_id, region):
    return f"https://{region}.api.riotgames.com/lol/match/v4/matches/{match_id}?api_key={RIOT_API_KEY}"


def get_champion_mastery(summoner_id, champion_id, region):
    return f"https://{region}.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-summoner/{summoner_id}/by-champion/{champion_id}?api_key={RIOT_API_KEY}"


# NA only at the moment
def get_summoner_mmr(summoner_name):
    return f"https://na.whatismymmr.com/api/v1/summoner?name={summoner_name}"
