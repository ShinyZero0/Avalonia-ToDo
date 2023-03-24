#!/bin/bash
scrpit_dir=$(dirname ${BASH_SOURCE[0]})
fd "axaml$" $scrpit_dir/../ | xargs xstyler -c $scrpit_dir/format.json -i --indent-size 2 -f -
