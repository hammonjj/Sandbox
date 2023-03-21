from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework import status
from RefuelApi.models import FillUp, Vehicle
from RefuelApi.serializers import FillupSerializer, VehicleSerializer

###########################################
# Vehicles
###########################################
@api_view(['GET', 'POST'])
def vehicle_list(request):

    if request.method == 'GET':
        vehicles = Vehicle.objects.all()
        serializer = VehicleSerializer(vehicles, many=True)
        return Response({"vehicles":serializer.data})
    
    elif request.method == 'POST':
        serializer = VehicleSerializer(data=request.data)

        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

@api_view(['GET', 'DELETE'])
def vehicle_detail(request, id):
    try:
        vehicle = Vehicle.objects.get(pk=id)
    except Vehicle.DoesNotExist:
        return Response(status=status.HTTP_404_NOT_FOUND)
    
    if request.method == 'GET':
        serializer = VehicleSerializer(vehicle)
        return Response(serializer.data)
    
    elif request.method == 'DELETE':
        vehicle.delete()
        return Response(status=status.HTTP_200_OK)

###########################################
# Fill Ups
###########################################
@api_view(['GET','POST'])
def fillup(request):
    if request.method == 'GET':
        fillups = FillUp.objects.all()
        serializer = FillupSerializer(fillups, many=True)
        return Response({"fillups":serializer.data})
    
    elif request.method == 'POST':
        serializer = FillupSerializer(data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET', 'DELETE', 'PUT'])
def fillup_detail(request, id):
    try:
        fillup = FillUp.objects.get(pk=id)
    except FillUp.DoesNotExist:
        return Response(status=status.HTTP_404_NOT_FOUND)

    if request.method == 'GET':
        serializer = FillupSerializer(fillup)
        return Response(serializer.data)
    
    elif request.method == 'DELETE':
        fillup.delete()
        return Response(status=status.HTTP_200_OK)
    
    elif request.method == 'PUT':
        serializer = FillupSerializer(fillup, data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)