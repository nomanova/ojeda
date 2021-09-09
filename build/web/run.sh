#!/bin/sh

# Replace API placeholder (nginx)
sed -i "s%__API_LOCATION__%$API_LOCATION%g" /etc/nginx/conf.d/default.conf

# Start nginx
nginx -g 'daemon off;'
