kubectl apply -f forum-service-deployment.yml
kubectl apply -f api-gateway-deployment.yml
kubectl apply -f account-service-deployment.yml
kubectl apply -f rabbitmq-deployment.yml
kubectl apply -f ingress-deployment.yml

pause