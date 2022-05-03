#!sh

kubectl delete -f deployments/sql-server.yaml
kubectl delete -f storageclasses/azure-disk.yaml
kubectl delete secret mssql

kubectl delete -f deployments/rabbitmq.yaml
kubectl delete -f deployments/smtp4dev.yaml