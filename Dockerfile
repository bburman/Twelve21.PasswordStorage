# This is a Docker multi-stage build, which first defines a build step and then
# copies the build output to a clean image

# It is constructed this way to optimize for fast iterative build times and
# small final images

## Build step
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as builder

WORKDIR /etc/password-storage

# Copy over application files and build project
COPY . .
RUN dotnet publish -c release --self-contained -r linux-musl-x64 -o /etc/password-storage/build

## Runtime step
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine

WORKDIR /etc/password-storage

# Copy the built files from the build step
COPY --from=builder /etc/password-storage/build .

# Link the application
# Create a non-privileged user to run as. Specify the GID and UID per convention
RUN ln -s /etc/password-storage/Twelve21.PasswordStorage /usr/local/bin/Twelve21.PasswordStorage && \
    addgroup -g 1221 -S twelve21 && \
    adduser  -g 1221 -S twelve21 -G twelve21

ENTRYPOINT ["Twelve21.PasswordStorage"]
