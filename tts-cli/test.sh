#!/bin/bash
# -*- coding: utf-8, tab-width: 2 -*-
"$(readlink -m "$BASH_SOURCE/../../cs-make-test.sh"
  )" serverTtsCli.cs |& less -S; exit $?
