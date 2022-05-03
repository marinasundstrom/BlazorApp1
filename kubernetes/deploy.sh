#!sh

kubectl create secret generic mssql --from-literal=SA_PASSWORD="PyQ2m5l&xP@ssw0rd"
kubectl apply -f storageclasses/azure-disk.yaml
kubectl apply -f deployments/sql-server.yaml

kubectl apply -f deployments/rabbitmq.yaml
kubectl apply -f deployments/smtp4dev.yaml
