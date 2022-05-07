class Summoner:
    def __init__(self, summoner_data):
        self.participantId = summoner_data["participantId"]
        self.currentAccountId = summoner_data["player"]["currentAccountId"]
        self.summonerId = summoner_data["player"]["summonerId"]
        self.accountId = summoner_data["player"]["accountId"]
        self.currentPlatformId = summoner_data["player"]["currentPlatformId"]
        self.summonerName = summoner_data["player"]["summonerName"]
