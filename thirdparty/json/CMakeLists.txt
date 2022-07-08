include(FetchContent)

FetchContent_Declare(json
    GIT_REPOSITORY https://github.com/nlohmann/json.git
    GIT_TAG v3.12.0
    GIT_SHALLOW TRUE
    GIT_PROGRESS TRUE
)

set(JSON_MultipleHeaders OFF)

FetchContent_MakeAvailable(json)