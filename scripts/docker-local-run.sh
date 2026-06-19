#!/usr/bin/env bash
set -euo pipefail

docker compose build

docker rm -f finshark-api 2>/dev/null || true
docker run -d \
  --name finshark-api \
  --restart unless-stopped \
  -p 5100:8080 \
  finshark

echo "Running finshark-api at http://localhost:5100"
