from django.db import models

class Vehicle(models.Model):
    make = models.CharField(max_length=50)
    model = models.CharField(max_length=50)
    startingMileage = models.PositiveIntegerField()
    purchaseDate = models.DateField()

    def __str__(self):
        return self.make + ' ' + self.model

class FillUp(models.Model):
    date = models.DateField()
    vehicle = models.ForeignKey(Vehicle, on_delete=models.CASCADE)
    gallonsPumped = models.DecimalField(max_digits=6, decimal_places=3)
    pricePerGallon = models.DecimalField(max_digits=6, decimal_places=3)
    milesDriven = models.DecimalField(max_digits=6, decimal_places=2)
    gasStationLong = models.DecimalField(max_digits=22, decimal_places=16)
    gasStationLat = models.DecimalField(max_digits=22, decimal_places=16)