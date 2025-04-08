pipeline {
    agent {
        kubernetes {
            yaml '''
apiVersion: v1
kind: Pod
spec:
  containers:
  - name: sonar-scanner
    image: sonarsource/sonar-scanner-cli
    command:
    - cat
    tty: true
  - name: kubectl
    image: bitnami/kubectl:latest
    command:
    - cat
    tty: true
    securityContext:
      runAsUser: 0
    volumeMounts:
    - name: kubeconfig-secret
      mountPath: /.kube/config
      subPath: kubeconfig
  - name: dind
    image: docker:dind
    args: ["--registry-mirror=https://mirror.gcr.io", "--storage-driver=overlay2"]
    securityContext:
      privileged: true  # Needed to run Docker daemon
    env:
    - name: DOCKER_TLS_CERTDIR
      value: ""  # Disable TLS for simplicity
    volumeMounts:
    - name: docker-config
      mountPath: /etc/docker/daemon.json
      subPath: daemon.json  # Mount the file directly here
  volumes:
  - name: docker-config
    configMap:
      name: docker-daemon-config
  - name: kubeconfig-secret
    secret:
      secretName: kubeconfig-secret
'''
        }
    }
        stages {
        stage('SonarQube Analysis') {
            steps {
                container('sonar-scanner') {
                    script {
                        // Run the SonarQube analysis using dotnet-sonarscanner
                        sh '''
                            dotnet sonarscanner begin /k:"reminder-list-dot-net" \
                                /d:sonar.host.url="http://my-sonarqube-sonarqube.school-ns.svc.cluster.local:9000" \
                                /d:sonar.token="sqp_1958971049a7e0c492e5f418668bb558c4d8273d" \
                                /d:sonar.sources=.
                            
                            dotnet build
                            
                            dotnet sonarscanner end /d:sonar.token="sqp_1958971049a7e0c492e5f418668bb558c4d8273d"
                        '''
                    }
                }
            }
        }
    }
}
