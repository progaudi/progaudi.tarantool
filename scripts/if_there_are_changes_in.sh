#!/usr/bin/env bash

# Usage:
# if_there_are_changes_in folder1 ... folderN --then script arg1 arg2 arg3 --else script2 arg1 arg2
# Purpose:
# Run script only if provided folders has changes against master.

set -e

# populate logic_folders
logic_folders=()
true_script=()
false_script=("true")
step="populate_folders"

for arg in "$@"
do
    if [ $arg == "--then" ]; then
        step="then"
        continue;
    fi

    if [ $arg == "--else" ]; then
        step="else"
        false_script=()
        continue;
    fi

    if [ $step == "populate_folders" ]; then
        logic_folders+=($arg)
    elif [ $step == "then" ]; then
        true_script+=($arg)
    else
        false_script+=($arg)
    fi
done

pushd ${BASH_SOURCE%/*}/..

commit=$(diff --old-line-format='' --new-line-format='' <(git rev-list --first-parent origin/master) <(git rev-list --first-parent HEAD) | head -1)
predicate_is_true=0
for folder in ${logic_folders[@]}; do
    echo "Check for changes in $folder"
    changes=$(git diff --relative=$folder $commit --quiet --; echo $?)

    if [ $changes = 1 ]; then
        predicate_is_true=1
        echo "There are changes in $folder"
    else
        echo "No changes in $folder"
    fi
done

popd

if [ $predicate_is_true = 1 ]; then
    ${true_script[@]}
else
    ${false_script[@]}
fi
