#!/usr/bin/env bash
set -euo pipefail

docker compose build
docker save finshark -o finshark.tar

echo "Saved finshark.tar — copy it to your production server, then run:"
echo "  docker load -i finshark.tar"
echo "  docker compose up -d"
