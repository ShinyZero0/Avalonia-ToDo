#!/bin/bash
scrpit_dir=$(dirname ${BASH_SOURCE[0]})
{ fd "\.a?xa?ml$" $scrpit_dir/../ ; fd "\.csproj$" $scrpit_dir/../ ; } | xargs xstyler -c $scrpit_dir/format.json -i --indent-size 2 -f -
