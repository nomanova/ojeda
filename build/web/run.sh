#!/bin/sh

# Replace API placeholder (nginx)
echo "Using API_LOCATION ${API_LOCATION}"
sed -i "s%__API_LOCATION__%$API_LOCATION%g" /etc/nginx/conf.d/default.conf

# Start nginx
echo "Starting nginx..."
nginx -g 'daemon off;'
