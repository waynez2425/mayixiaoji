#!/bin/sh
set -eu

pandoc -s -o index.html -c css/classless.css -c css/piglet.css \
       --highlight-style haddock --filter pandoc-crossref \
       --lua-filter pandoc-filters/code-block-captions.lua \
       index.md

patch --no-backup-if-mismatch -p1 < patches/highlight-code.patch
