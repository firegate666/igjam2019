def buildPath
def uploadArtifact
def globalBuildNumber = 0

def getCommandOutput(cmd) {
  if (isUnix()){
       return sh(returnStdout:true , script: '#!/bin/sh -e\n' + cmd).trim()
   } else{
     stdout = bat(returnStdout:true , script: cmd).trim()
     result = stdout.readLines().drop(1).join("")
     return result
  }
}

pipeline {
    agent any

    environment {
        APP_NAME = "Planex"
    }
    
    options {
        buildDiscarder logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '', daysToKeepStr: '7', numToKeepStr: '20')
        disableConcurrentBuilds()
        timestamps()
    }

    parameters {
        gitParameter branch: '', branchFilter: '.*', defaultValue: 'origin/master', name: 'BRANCH_OR_TAG', quickFilterEnabled: true, selectedValue: 'DEFAULT', sortMode: 'ASCENDING_SMART', tagFilter: '*', type: 'GitParameterDefinition'
        choice choices: ['Win', 'Win64', 'OSXUniversal', 'Linux64', 'iOS', 'Android', 'WebGL', 'WindowsStoreApps', 'tvOS'], name: 'BUILD_TARGET'
        booleanParam 'IL2CPP_BUILD'
        booleanParam defaultValue: false, name: 'CLEAN_BUILD'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scmGit(branches: [[name: params.BRANCH_OR_TAG]], extensions: [], userRemoteConfigs: [[credentialsId: 'github_firegate666', url: 'git@github.com:firegate666/igjam2019.git']])
            }
        }

        stage('Unity Version') {
            steps {
                script {
                    def commandOutput = getCommandOutput("cat ProjectSettings/ProjectVersion.txt | grep ^m_EditorVersion:")
                    def unityVersion = commandOutput.split(":")[1].trim()
                    env.UNITY_VERSION = "${unityVersion}"
                    env.UNITY_PATH = "/Applications/Unity/Hub/Editor/${unityVersion}/Unity.app/Contents/MacOS/Unity"

                    echo "Unity Version Found: ${unityVersion}"
                }
            }
        }

        stage('Generate Build Number') {
          steps {
            script {
              echo "Fetching global build number"
              def jobResult = build('Global-Build-Number')
              globalBuildNumber = jobResult.number
              echo "Global build number is: ${globalBuildNumber}"
            }
          }
        }
        
        stage('Update build name') {
            steps {
                buildDescription "Build ${params.BUILD_TARGET} (${globalBuildNumber})"
            }
        }

        stage('Set Paths') {
            steps {
                script {
                    def uploadArtifactPart = "Build/${env.UNITY_VERSION}/${params.BUILD_TARGET}"
                    buildPath = "${env.WORKSPACE}/${uploadArtifactPart}"
                    uploadArtifact = "**/${uploadArtifactPart}"
                }
            }
        }
        
        stage("Clean build") {
            when { expression { return params.CLEAN_BUILD == true } }
            steps {
                script {
                    sh "git clean -fdx"
                }
            }
        }

        stage('Build') {
            steps {
                withCredentials([file(credentialsId: 'KEYSTORE_PATH', variable: 'KEYSTORE_PATH'), string(credentialsId: 'KEYSTORE_PASS', variable: 'KEYSTORE_PASS'), string(credentialsId: 'KEYSTORE_ALIAS_NAME', variable: 'KEYSTORE_ALIAS_NAME')]) {
                    script {
                        def il2cpp = "";
                        if(params.IL2CPP_BUILD) {
                            il2cpp = "-il2cpp 1";
                        }

                        def devBuild = "";
                        if (params.FORCE_DEVELOPMENT_BUILD) {
                            devBuild = "-forceDebug"
                        }

                        sh "mkdir -p \"${buildPath}\""

                        sh "${env.UNITY_PATH} -executeMethod Builder.Build -keystorePath '${KEYSTORE_PATH}' -keystorePass '${KEYSTORE_PASS}' -keyaliasPass '${KEYSTORE_PASS}' -keyaliasName '${KEYSTORE_ALIAS_NAME}' -buildTarget ${params.BUILD_TARGET} -projectPath ${env.WORKSPACE} -buildNumber ${globalBuildNumber} -quit -batchmode -outputFolderName ${buildPath}/${env.APP_NAME} -quitTimeout 3600 ${devBuild} ${il2cpp} -logFile - "
                    }
                }
            }
        }
        
        stage('iOSClean') {
            when { expression { return "${params.BUILD_TARGET}" == "iOS" } }
            steps {
                script {
                    def buildName = env.APP_NAME
                    def buildArtifact = "${buildPath}/${buildName}"
                    
                    echo "xCode Clean Project starting..."
                    sh """
                        cd ${buildArtifact}
                        /usr/bin/xcodebuild -allowProvisioningUpdates -project Unity-iPhone.xcodeproj -scheme Unity-iPhone -sdk iphoneos -configuration Release clean
                    """
                    echo "xCode Clean Project finished..."
                }
            }
        }
        
        stage('iOSArch') {
            when { expression { return "${params.BUILD_TARGET}" == "iOS" } }
            steps {
                script {
                    def buildName = env.APP_NAME
                    def buildArtifact = "${buildPath}/${buildName}"

                    echo "Create Archive starting..."
                    sh """
                        cd ${buildArtifact}
                        /usr/bin/xcodebuild -allowProvisioningUpdates -project Unity-iPhone.xcodeproj -scheme Unity-iPhone -sdk iphoneos -configuration Release archive -archivePath '${env.WORKSPACE}/ios-build/***.xcarchive' clean
                    """    
                    echo "Create Archive finished..."
                }
            }
        }        
        
        stage('iOSipa') {
            when { expression { return "${params.BUILD_TARGET}" == "iOS" } }
            steps {
                script {
                    def buildName = env.APP_NAME
                    def buildArtifact = "${buildPath}/${buildName}"
                
                    echo "Create ipa starting..."
                    sh """
                        cd ${env.WORKSPACE}/ios-build
                        /usr/bin/xcodebuild -allowProvisioningUpdates -exportArchive -archivePath '***.xcarchive'  -exportPath '.' -allowProvisioningUpdates -exportOptionsPlist '${env.WORKSPACE}/ios-conf/exportOptions.plist'
                    """
                    echo "Create ipa finished..."
                }
            }
        }

        stage('iOSTestFlight') {
            when { expression { return "${params.BUILD_TARGET}" == "iOS" } }
            steps {
                withCredentials([string(credentialsId: 'ITUNES_USERNAME', variable: 'ITUNES_USERNAME'), string(credentialsId: 'ITUNES_PASSWORD', variable: 'ITUNES_PASSWORD')]) {
                    script {
                        def buildName = env.APP_NAME
                        def buildArtifact = "${buildPath}/${buildName}"
                        def ipaName = "${buildName}.ipa"
                        def targetIpaName = "${buildName}-${globalBuildNumber}.ipa"
                        
                        echo "TesfFlight upload starting"
                        sh """
                            cd ${env.WORKSPACE}/ios-build
                            /usr/bin/xcrun altool --upload-app --type ios --file ${ipaName} --username ${ITUNES_USERNAME} --password ${ITUNES_PASSWORD}
                            
                            cp ${ipaName} ${buildArtifact}/../${targetIpaName}
                        """
                        echo "TesfFlight upload completed"

                        uploadArtifact = "${uploadArtifact}/${targetIpaName}"    
                        echo "upload artifact from ${uploadArtifact}"
                    }
                }
            }
        }
        
        stage('Android rename build artifact') {
            when { expression { return "${params.BUILD_TARGET}" == "Android" } }
            steps {
                echo "Upload ${params.BUILD_TARGET} app bundle"

                script {
                    def buildName = env.APP_NAME
                    def buildArtifact = "${buildPath}/${buildName}"
                    def apkName = "${buildName}-${globalBuildNumber}.aab"

                    sh "cp ${buildArtifact} ${buildPath}/${apkName}"

                    uploadArtifact = "${uploadArtifact}/${apkName}"
                }
            }
        }
        
        stage('Win Mono zip Package') {
            when { expression { return "${params.BUILD_TARGET}" == "Win" } }
            steps {
                echo "Create ${params.BUILD_TARGET} zip file"

                script {
                    def buildName = env.APP_NAME
                    def srcDirectory = "${buildPath}/"
                    def zipFileName = "${buildName}-${globalBuildNumber}.zip"
                    def zipFileTempPath = "${buildPath}/../${zipFileName}"
                    def zipFilePath = "${buildPath}/${zipFileName}"

                    sh "mv ${srcDirectory}/${buildName} ${srcDirectory}/${buildName}.exe"

                    sh "rm -f ${buildPath}/*.zip"
                    sh "ditto -c -k --sequesterRsrc ${srcDirectory} ${zipFileTempPath}"
                    sh "mv ${zipFileTempPath} ${zipFilePath}"

                    uploadArtifact = "${uploadArtifact}/${zipFileName}"
                }
            }
        }
      
        stage('WebGL zip Package') {
            when { expression { return "${params.BUILD_TARGET}" == "WebGL" } }
            steps {
                echo "Create ${params.BUILD_TARGET} zip file"

                script {
                    def buildName = env.APP_NAME
                    def srcDirectory = "${buildPath}/${buildName}"
                    def zipFileName = "${buildName}-${globalBuildNumber}.zip"
                    def zipFilePath = "${buildPath}/${zipFileName}"

                    sh "ls -la ${buildPath}"
                    sh "ditto -c -k --sequesterRsrc ${srcDirectory} ${zipFilePath}"

                    uploadArtifact = "${uploadArtifact}/${zipFileName}"
                }
            }
        }

        stage('OSXUniversal dmg Package') {
            when { expression { return "${params.BUILD_TARGET}" == "OSXUniversal" } }
            steps {
                echo "Create ${params.BUILD_TARGET} disk image"

                script {
                    def buildName = env.APP_NAME;
                    def fileName = "${buildName}-${globalBuildNumber}-installer.dmg"
                    def dmgName = "${buildPath}/${fileName}"

                    uploadArtifact = "${uploadArtifact}/${fileName}"

                    sh "rm -f \"${dmgName}\""
                    sh "create-dmg --volname \"${env.APP_NAME} Installer\" --window-pos 200 120 --window-size 800 400 --icon-size 100 --app-drop-link 600 185 --skip-jenkins \"${dmgName}\" \"${buildPath}/${env.APP_NAME}.app\""

                }
            }
        }
    }

    post {
        always {
            script {
                logParser failBuildOnError: true, projectRulePath: 'logparser_rules.txt', unstableOnWarning: false, useProjectRule: true
                s3Upload consoleLogLevel: 'INFO', dontSetBuildResultOnFailure: false, dontWaitForConcurrentBuildCompletion: false, entries: [[bucket: "planex-artifacts/${params.BUILD_TARGET}", excludedFile: '', flatten: true, gzipFiles: false, keepForever: false, managedArtifacts: false, noUploadOnFailure: true, selectedRegion: 'eu-central-1', showDirectlyInBrowser: false, sourceFile: uploadArtifact, storageClass: 'STANDARD', uploadFromSlave: false, useServerSideEncryption: false]], pluginFailureResultConstraint: 'FAILURE', profileName: 'Tappy-Unicorn-Artifacts', userMetadata: []
            }
        }
    }
}
