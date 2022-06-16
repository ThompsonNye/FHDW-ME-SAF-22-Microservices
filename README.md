# FHDW-ME-SAF-22-Microservices
Microservices Anwendung zur Studienarbeit in SAF im Sommersemester '22 an der FHDW Mettmann.

## Nutzung

### Voraussetzung

Das Deployment ist auf die Nutzung von Docker Swarm ausgerichet, d.h. Docker Swarm muss zuvor auf allen gewünschten Maschinen initialisiert worden sein:

Auf dem Master-Knoten:

```sh
docker swarm init
```

Auf allen Worker-Knoten muss anschließend der Befehl ausgeführt werden, der beim Initialisieren auf dem Master-Knoten ausgegeben wurde. Dieser sieht in etwa so aus:

```sh
docker swarm join --token SWMTKN-1-108ffwsd5nqvf2yqndpi7pft6k0lpgxiva9o2lreumjuyjctb6-a4um0chtcjtujysptyp05slhj 1.2.3.4:2377
```

### Deployment

Standard (ein Knoten, keine Repliken):

```sh
docker stack deploy -c docker-compose.yaml saf
```

Mit Repliken:

```sh
# 2 Replicas
docker stack deploy -c docker-compose.yaml -c docker-compose.replicas2.yaml saf

# 4 Replicas
docker stack deploy -c docker-compose.yaml -c docker-compose.replicas4.yaml saf
```

Auf mehreren Knoten (Beispiel nutzt 6 Knoten, 1 Master und 5 Worker):

```sh
docker stack deploy -c docker-compose.yaml -c docker-compose.distributed.yaml saf
```
