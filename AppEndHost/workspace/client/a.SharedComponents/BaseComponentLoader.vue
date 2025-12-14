<template>
    <component :is="comp" :cid="cid" :id="cid" :ismodal="ismodal" />
</template>
<script>
    let _this = { cid: "", uid: "", ismodal: "", c: null };
    let ismodal = "false";
    import { shallowRef } from "vue";
    export default {
        setup(props) {
            const comp = shallowRef();
            let src = props['src'];
            const originalSrc = src;
            const cid = props['cid'] || 'unknown';
            
            console.log(`[ComponentLoader] Setup called for cid="${cid}", original src="${originalSrc}"`);
            
            _this.uid = fixNull(props['uid'], genUN('c_'));
            _this.ismodal = props['ismodal'];
            ismodal = props['ismodal'];
            
            if (src.startsWith('qs:')) {
                src = getQueryString(src.replace('qs:', ''));
                console.log(`[ComponentLoader] Query string resolved: "${src}"`);
            }
            
            try {
                let defaultComp = "components/Home";
                if (typeof shared !== 'undefined' && typeof shared.getAppConfig === 'function') {
                    try {
                        let appConfig = shared.getAppConfig();
                        console.log(`[ComponentLoader] App config:`, appConfig);
                        if (appConfig && appConfig.defaultComponent) {
                            defaultComp = appConfig.defaultComponent;
                            console.log(`[ComponentLoader] Using defaultComponent from app.json: "${defaultComp}"`);
                        }
                    } catch (e) {
                        console.warn('[ComponentLoader] Could not load app config, using default:', e);
                    }
                } else {
                    console.warn('[ComponentLoader] shared.getAppConfig is not available');
                }
                
                const srcBeforeFix = src;
                src = fixEndBy(fixNull(src, defaultComp), ".vue");
                console.log(`[ComponentLoader] Path resolution: "${srcBeforeFix}" -> "${src}"`);
                
                // Check if loadVM exists
                if (typeof loadVM === 'undefined') {
                    console.error('[ComponentLoader] ERROR: loadVM function is not defined!');
                } else {
                    console.log(`[ComponentLoader] Calling loadVM("${src}") for cid="${cid}"`);
                }
                
            } catch (e) {
                console.error('[ComponentLoader] Error in BaseComponentLoader setup:', e);
                src = fixEndBy(fixNull(src, "components/Home"), ".vue");
            }
            
            try {
                // Create a custom loadVM wrapper with detailed logging
                const wrappedLoadVM = (path) => {
                    console.log(`[ComponentLoader] Creating wrapped loadVM for path: "${path}", cid="${cid}"`);
                    
                    const { loadModule } = window["vue3-sfc-loader"];
                    if (!loadModule) {
                        console.error(`[ComponentLoader] vue3-sfc-loader.loadModule is not available!`);
                        return null;
                    }
                    
                    const options = {
                        moduleCache: { vue: Vue },
                        getFile(url) {
                            const fullFetchUrl = url + "?fake=" + (shared?.fake || '');
                            console.log(`[ComponentLoader] [FETCH] Fetching: "${fullFetchUrl}" for cid="${cid}"`);
                            
                            return fetch(fullFetchUrl)
                                .then(resp => {
                                    console.log(`[ComponentLoader] [FETCH] Response for "${fullFetchUrl}":`, {
                                        ok: resp.ok,
                                        status: resp.status,
                                        statusText: resp.statusText,
                                        url: resp.url,
                                        cid: cid
                                    });
                                    
                                    if (resp.ok) {
                                        return resp.text().then(i => {
                                            const trimmed = trimUi(i);
                                            console.log(`[ComponentLoader] [FETCH] SUCCESS for "${fullFetchUrl}", content length: ${trimmed.length}, cid="${cid}"`);
                                            return trimmed;
                                        });
                                    } else {
                                        console.error(`[ComponentLoader] [FETCH] FAILED for "${fullFetchUrl}":`, {
                                            status: resp.status,
                                            statusText: resp.statusText,
                                            cid: cid
                                        });
                                        return Promise.reject(new Error(`HTTP ${resp.status}: ${resp.statusText} - ${fullFetchUrl}`));
                                    }
                                })
                                .catch(error => {
                                    console.error(`[ComponentLoader] [FETCH] ERROR for "${fullFetchUrl}":`, error);
                                    console.error(`[ComponentLoader] [FETCH] Error details:`, {
                                        message: error?.message,
                                        stack: error?.stack,
                                        url: fullFetchUrl,
                                        cid: cid
                                    });
                                    throw error;
                                });
                        },
                        addStyle(styleStr) {
                            const style = document.createElement("style");
                            style.textContent = styleStr;
                            const ref = document.head.getElementsByTagName("style")[0] || null;
                            document.head.insertBefore(style, ref);
                            console.log(`[ComponentLoader] [STYLE] Added styles for cid="${cid}"`);
                        }
                    };
                    
                    const asyncComponent = Vue.defineAsyncComponent(() => {
                        console.log(`[ComponentLoader] [LOAD] Calling loadModule("${path}") for cid="${cid}"`);
                        return loadModule(path, options)
                            .then(module => {
                                console.log(`[ComponentLoader] [LOAD] SUCCESS for "${path}", cid="${cid}"`, module);
                                return module;
                            })
                            .catch(error => {
                                console.error(`[ComponentLoader] [LOAD] ERROR for "${path}", cid="${cid}":`, error);
                                console.error(`[ComponentLoader] [LOAD] Error details:`, {
                                    message: error?.message,
                                    stack: error?.stack,
                                    url: path,
                                    cid: cid
                                });
                                throw error;
                            });
                    });
                    
                    return asyncComponent;
                };
                
                comp.value = wrappedLoadVM(src);
                console.log(`[ComponentLoader] loadVM wrapper returned component for "${src}", cid="${cid}"`);
                
            } catch (e) {
                console.error(`[ComponentLoader] ERROR calling wrapped loadVM("${src}") for cid="${cid}":`, e);
                console.error(`[ComponentLoader] Error stack:`, e?.stack);
            }
            
            return { comp };
        },
        created() { 
            _this.c = this; 
            _this.cid = _this.uid;
            console.log(`[ComponentLoader] Created for uid="${_this.uid}", cid="${_this.cid}"`);
        },
        mounted() {
            console.log(`[ComponentLoader] Mounted for uid="${_this.uid}", cid="${_this.cid}"`);
            if (fixNull(_this.uid, '') !== '') {
                $(document).ready(function () {
                    let testDone = function () {
                        if (ismodal === "true") {
                            if ($("#" + _this.uid).attr("ae-data-ready") === "true") {
                                clearInterval(testInterval);
                                console.log(`[ComponentLoader] Initializing modal component for uid="${_this.uid}"`);
                                initVueComponent(_this);
                            }
                        } else {
                            clearInterval(testInterval);
                            console.log(`[ComponentLoader] Initializing component for uid="${_this.uid}"`);
                            initVueComponent(_this);
                        }
                    };
                    let testInterval = setInterval(testDone, 50);
                });
            } else {
                console.warn(`[ComponentLoader] No uid provided for component`);
            }
        },
        props: { src: String, uid: String, ismodal: String, cid: String }
    }
</script>