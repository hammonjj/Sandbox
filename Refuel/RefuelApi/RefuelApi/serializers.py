from rest_framework import serializers
from .models import Vehicle
from .models import FillUp

class VehicleSerializer(serializers.ModelSerializer):
    class Meta:
        model = Vehicle
        fields = ['id', 'make', 'model', 'startingMileage', 'purchaseDate']

class FillupSerializer(serializers.ModelSerializer):
    class Meta:
        model = FillUp
        fields = ['id', 'date', 'vehicle', 'gallonsPumped', 'pricePerGallon', 'milesDriven', 'gasStationLong', 'gasStationLat']