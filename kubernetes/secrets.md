kubectl create secret generic mssql --from-literal=SA_PASSWORD="PyQ2m5l&xP@ssw0rd"
kubectl delete secret binding-production-sqlserver-secret

kubectl create secret generic binding-production-rabbitmq-rabbit-secret --namespace default --from-literal=protocol=<value> --from-literal=host=<value> --from-literal=port=<value>

kubectl rollout restart deployment mssql-deployment

az acr login -n ba1cregistry.azurecr.io

az aks update -n blazorapp1cluster1 -g Blazor-App1 --attach-acr ba1cregistry

Server=10.0.157.173,1433;User Id=sa;Password=PyQ2m5l&xP@ssw0rd;Database=BlazorApp1;

amqp://10.0.196.115:5672
http://10.0.196.115:15672

http://10.0.72.149:80
smtp://10.0.72.149:25