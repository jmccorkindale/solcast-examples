import json
import requests

SOLCAST_API_KEY = "[Insert your API Key here]"
SOLCAST_API_URL = "https://api.solcast.com.au/rooftop_sites"


class RooftopSite:
    """
    To send rooftop sites to the Solcast API, the following attributes need to be present in the payload:
        *name: a friendly display name for the site
        *longitude: longitude of the site
        *latitude: latitude of the site
        *azimuth (optional): azimuth of the site (180 to -180) where 0 is North
        *tilt (optional): tilt of the site (0 to 90) where 0 is flat
    """
    def __init__(self, name: str, longitude: float, latitude: float, azimuth: float, tilt: float):
        self.name = name
        self.longitude = longitude
        self.latitude = latitude
        self.azimuth = azimuth
        self.tilt = tilt

    def to_json(self) -> str:
        """
        Converts the rooftop site to a json in the required format for the Solcast API:
        i.e.
        {
            "site" : {
                "name" : "My Site",
                "longitude" : -149.117,
                "latitude" : 35.2
            }
        }
        """
        return json.dump('{{ "site": {} }}', default=lambda x: x.__dict__)


def add_rooftop_to_solcast(rooftop: RooftopSite) -> requests.Response:
    """
    Using the requests python module, performs a post request with a json payload to the Solcast API.
    :param rooftop: the rooftop site instance to be sent to the Solcast API
    :return: the response of the request
    """
    url = str.format("{}?format=json&api_key={}", SOLCAST_API_URL, SOLCAST_API_KEY)
    response = requests.post(url=url, data=rooftop.to_json(), headers={"Content-Type": "application/json"})
    return response

def main():
    rooftop = RooftopSite("My Site", -149.117, 35.2)
    response = add_rooftop_to_solcast(rooftop)
    if response.ok:
        result = response.json()
        return result["site"]["resource_id"]
    else:
        return None